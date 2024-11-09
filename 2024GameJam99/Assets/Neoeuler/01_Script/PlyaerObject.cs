using UnityEngine;

public class PlayerObject : MonoBehaviour
{
    public float moveSpeed = 5f; // 플레이어 이동 속도
    private Rigidbody2D rb;
    private Vector2 movement;

    
    public float attackCooldown = 1f; // 공격 쿨다운 (초)
    public float damageAmount = 10f; // 공격 데미지
    public float infectionAmount = 10f;

    private float attackTimer = 0f; // 공격 쿨다운 타이머
    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 방향키 입력 받기
        movement.x = Input.GetAxisRaw("Horizontal"); // 왼쪽/오른쪽
        movement.y = Input.GetAxisRaw("Vertical"); // 위/아래
        
        attackTimer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        // 이동 로직
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
    
    //public float damageAmount = 10f; // ChickenObject에 줄 데미지
    void OnCollisionStay2D(Collision2D collision)
    {
        // 충돌한 객체가 ChickenObject인지 확인
        ChickenObject chicken = collision.gameObject.GetComponent<ChickenObject>();
        if (chicken != null && attackTimer <= 0f) // 공격 쿨다운이 끝난 경우에만 공격
        {
            // ChickenObject에 데미지를 입힘
            chicken.TakeDamage(damageAmount, infectionAmount);

            // 공격 후 쿨다운 초기화
            attackTimer = attackCooldown;
        }
    }

    public void TakeDamage(float amount)
    {
        
    }
    
    
}