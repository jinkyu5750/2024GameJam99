using System;
using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum ObjectState
{
    Walk,
    Cough,
    Sit,
}

public class ChickenObject : MonoBehaviour
{
    public Transform player;
    public float evadeSpeed = 5f;
    public float safeDistance = 10f;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;
    public Color damagedColor = Color.red;
    public float randomMoveInterval = 2f; // 기본 이동 주기
    public float randomMoveDistance = 1f; // 기본 이동 거리
    public float smoothMoveSpeed = 2f;
    public Color infectedColor = Color.magenta;
    public float chickenDamage = 10f;
    
    // 체력과 감염 수치
    public float maxHealth = 100f;
    private float currentHealth;
    public float infectionLevel { get; private set; }
    public float maxInfectionLevel = 100f;

    public float infectionAmount = 5f;
    public float infectionDamageOverTime = 5f; // 초당 감염 데미지
    private bool isTakingInfectionDamage = false; // 감염 데미지 적용 중 여부

    private Coroutine infectionDamageCoroutine; // 감염 데미지 코루틴 핸들
    
    // UI 요소
    public Image hpFill;
    public Image infectionFill;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isKnockedBack = false;
    private float knockbackTimer = 0f;

    private Coroutine randomMoveCoroutine;
    private Vector2 targetPosition;
    
    public float moveSpeed = 5f;
    public bool IsInfected { get; private set; } = false;

    public float infectDistance = 2.5f;
    private float infectedMoveSpeedMultiplier = 2f; // 감염 시 이동 속도 배율
    private float infectedMoveDistanceMultiplier = 2f; // 감염 시 이동 거리 배율
    private float infectedMoveIntervalDivider = 2f; // 감염 시 이동 주기 나누기 배율
    
    private float directionUpdateTimer; // 방향 업데이트를 위한 타이머
    private Vector2 lastDirection;
    private Vector2 previousPosition; // 이전 프레임 위치 저장
    // 이동 방향에 따라 Sprite를 Flip
    public float directionUpdateInterval = 0.1f; // 방향 업데이트 주기 (초)

    private SpriteAnimation spriteAnimation;
    
    private bool isStopped = false; // 이동 중지 여부

    private Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteAnimation = GetComponent<SpriteAnimation>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        currentHealth = maxHealth;
        infectionLevel = 0f;

        UpdateHealthUI();
        UpdateInfectionUI();
        
        randomMoveCoroutine = StartCoroutine(RandomMovement()); // 초기 이동 시작
        
        // 시작할 때 Fill UI를 숨기기
        if (hpFill != null) hpFill.transform.parent.gameObject.SetActive(false);
        if (infectionFill != null) infectionFill.transform.parent.gameObject.SetActive(false);
        
        GameManager.Instance.RegisterChicken(this); // 게임 매니저에 등록
        
        previousPosition = transform.position; // 시작 위치 설정
        directionUpdateTimer = directionUpdateInterval; // 타이머 초기화
    }

    public void Init()
    {
        
    }

    void OnDestroy()
    {
        GameManager.Instance.UnregisterChicken(this); // 매니저에서 자신을 제거
    }

    void Update()
    {
        if (isStopped) return;
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;
            if (knockbackTimer <= 0f)
            {
                isKnockedBack = false;
            }
        }
        else if (CanMoveRandomly())
        {
            rb.MovePosition(Vector2.Lerp(rb.position, targetPosition, smoothMoveSpeed * Time.deltaTime));
        }
        else
        {
            EvadePlayer();
        }

        // 타이머가 interval에 도달할 때만 방향 업데이트 수행
        directionUpdateTimer -= Time.deltaTime;
        if (directionUpdateTimer <= 0f)
        {
            UpdateSpriteDirection();
            directionUpdateTimer = directionUpdateInterval; // 타이머 리셋
        }

        previousPosition = transform.position; // 현재 위치를 이전 위치로 업데이트
    }
    
    

    // ChickenObject의 모든 움직임을 중지하는 메서드
    public void StopMovement()
    {
        isStopped = true;

        // 모든 이동 관련 코루틴 중지
        if (randomMoveCoroutine != null) StopCoroutine(randomMoveCoroutine);

        // Rigidbody의 속도를 0으로 설정하고, 물리 작용을 무시하도록 설정
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;

        // Collider 비활성화하여 충돌 차단
        GetComponent<Collider2D>().enabled = false;
    }

 
    // 이동 방향에 따라 Sprite를 Flip
    // 이동 방향에 따라 Sprite를 Flip
    private void UpdateSpriteDirection()
    {
        Vector2 currentPosition = transform.position;
        Vector2 direction = currentPosition - previousPosition; // 이전 위치와 현재 위치 차이 계산

        if (direction.x < 0) // 왼쪽으로 이동할 때
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x > 0) // 오른쪽으로 이동할 때
        {
            spriteRenderer.flipX = true;
        }
    }
    
    
    // 감염 상태로 변경
    public void Infect()
    {
        if (IsInfected) return;

        IsInfected = true;
        if (spriteRenderer != null)
        {
            spriteRenderer.color = infectedColor;
            originalColor = infectedColor;
        }
        
        // 감염된 치킨은 이동 속도와 거리, 주기를 변경하고 주기적으로 기침을 시작
        UpdateInfectedMovementSettings();
        StartCoroutine(StartCoughing());
        // 초당 감염 데미지 적용 시작
        StartInfectionDamageOverTime();
    }
    
    // 초당 감염 데미지를 받는 코루틴 시작
    private void StartInfectionDamageOverTime()
    {
        if (!isTakingInfectionDamage)
        {
            isTakingInfectionDamage = true;
            infectionDamageCoroutine = StartCoroutine(ApplyInfectionDamageOverTime());
        }
    }

    // 초당 감염 데미지를 적용하는 코루틴
    private IEnumerator ApplyInfectionDamageOverTime()
    {
        while (IsInfected && currentHealth > 0)
        {
            TakeDamage(infectionDamageOverTime); // 초당 데미지 적용
            yield return new WaitForSeconds(1f);
        }
        isTakingInfectionDamage = false; // 감염 상태가 끝나면 종료
    }

    // 감염 치료
    public void CureInfection()
    {
        if (IsInfected)
        {
            IsInfected = false;
            if (infectionDamageCoroutine != null)
            {
                StopCoroutine(infectionDamageCoroutine); // 감염 데미지 중지
                infectionDamageCoroutine = null;
            }

            // 색상 복원
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }
        }
    }

    // 감염 상태에서 이동 속도, 거리, 주기를 변경
    private void UpdateInfectedMovementSettings()
    {
        moveSpeed *= infectedMoveSpeedMultiplier; // 이동 속도 증가
        randomMoveDistance *= infectedMoveDistanceMultiplier; // 이동 거리 증가
        randomMoveInterval /= infectedMoveIntervalDivider; // 이동 주기 감소 (더 자주 이동)

        // 기존의 랜덤 이동 코루틴을 멈추고 새로운 주기와 거리로 재시작
        if (randomMoveCoroutine != null)
        {
            StopCoroutine(randomMoveCoroutine);
        }
        randomMoveCoroutine = StartCoroutine(RandomMovement());
    }

    // 주기적으로 기침을 하며 주변 치킨에게 감염을 전달하는 코루틴
    private IEnumerator StartCoughing()
    {
        while (IsInfected)
        {
            yield return new WaitForSeconds(Random.Range(2f, 3f));
            
            CoughAndInfect();

        }
    }

    // 일정 거리 내의 비감염 치킨을 감염시키는 기침 메서드
    private void CoughAndInfect()
    {
        foreach (var chicken in GameManager.Instance.GetAllChickens())
        {
            if (chicken != this && !chicken.IsInfected)
            {
                float distance = Vector2.Distance(transform.position, chicken.transform.position);
                if (distance <= infectDistance)
                {
                    chicken.TakeInfection(infectionAmount);
                }
            }
        }
    }
    
    IEnumerator RandomMovement()
    {
        while (!isStopped)
        {
            if (CanMoveRandomly())
            {
                SetRandomTargetPosition();
            }
            yield return new WaitForSeconds(randomMoveInterval);
        }
    }
    
    private void SetRandomTargetPosition()
    {
        Vector2 randomDirection = GetRandomDirection();
        targetPosition = rb.position + randomDirection * randomMoveDistance;
    }

    private bool CanMoveRandomly()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        return !isStopped && distanceToPlayer >= safeDistance && !isKnockedBack;
    }

    private Vector2 GetRandomDirection()
    {
        return new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized;
    }

    void EvadePlayer()
    {
        if (isStopped) return;
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance < safeDistance)
        {
            Vector2 evadeDirection = (transform.position - player.position).normalized;
            rb.MovePosition(rb.position + evadeDirection * evadeSpeed * Time.deltaTime);
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            collision.gameObject.GetComponent<PlayerObject>().TakeDamage(chickenDamage);
            
            isKnockedBack = true;
            knockbackTimer = knockbackDuration;
        }
    }

    // 체력 감소 및 감염 처리
    public void TakeDamage(float damageAmount, float infect = 0)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
            }
            damageCoroutine = StartCoroutine(DamagedColorCoroutine());
        }

        TakeInfection(infect);
    }

    private Coroutine damageCoroutine; // DamageColor 코루틴 핸들

// 데미지를 받은 후 잠시 색상을 변경하고 원래 색상으로 되돌리는 코루틴
    private IEnumerator DamagedColorCoroutine()
    {
        DamagedColor(); // 색상을 변경
        yield return new WaitForSeconds(0.2f);
        ResetColor(); // 원래 색상으로 복원
    }

    void DamagedColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = damagedColor;
        }
    }

    void ResetColor()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
    
    public void TakeInfection(float amount)
    {
        infectionLevel += amount;
        infectionLevel = Mathf.Clamp(infectionLevel, 0, maxInfectionLevel);

        if (infectionLevel == maxInfectionLevel)
        {
            Infect();
        }
        
        UpdateInfectionUI();
    }

    public void Die()
    {
        Debug.Log("ChickenObject가 사망했습니다.");
        gameObject.SetActive(false);
        
    }

    private void UpdateHealthUI()
    {
        if (hpFill != null)
        {
            hpFill.fillAmount = currentHealth / maxHealth;
            hpFill.transform.parent.gameObject.SetActive(true);
        }
    }

    private void UpdateInfectionUI()
    {
        if (infectionFill != null)
        {
            infectionFill.fillAmount = infectionLevel / maxInfectionLevel;
            infectionFill.transform.parent.gameObject.SetActive(true);
            hpFill.transform.parent.gameObject.SetActive(true);
        }
    }
}
