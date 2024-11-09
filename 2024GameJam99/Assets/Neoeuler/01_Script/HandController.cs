using System.Collections;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public float descendSpeed = 5f; // 손이 내려오는 속도
    public float ascendSpeed = 7f; // 손이 올라가는 속도
    public float grabHeight = 3f; // 손이 도달할 위치 (지면으로부터)
    public float xRange = 5f; // 새로운 x 위치 범위
    
    private GameObject targetObject = null; // 잡을 대상
    private bool isGrabbing = false; // 현재 잡기 동작 중인지 여부
    private Coroutine grabCoroutine;

    void Update()
    {
        // Space 키 입력으로 동작 시작
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrabbing)
            {
                ReleaseTargetAndReturnToTop(); // 현재 타겟을 놓고 상단으로 돌아감
            }
            else
            {
                grabCoroutine = StartCoroutine(GrabTarget());
            }
        }
    }

    private IEnumerator GrabTarget()
    {
        isGrabbing = true;

        // 가장 가까운 타겟 찾기
        FindClosestTarget();

        if (targetObject != null)
        {
            // 타겟의 x 위치로 이동 후 내려오기
            transform.position = new Vector3(targetObject.transform.position.x, transform.position.y, transform.position.z);

            // 손이 타겟 위치까지 내려감
            while (transform.position.y > grabHeight)
            {
                transform.position += Vector3.down * descendSpeed * Time.deltaTime;
                yield return null;
            }

            // 타겟의 StopMovement 호출 및 자식으로 설정
            if (targetObject.TryGetComponent(out ChickenObject chicken))
            {
                chicken.StopMovement();
            }
            else if (targetObject.TryGetComponent(out PlayerObject player))
            {
                player.StopMovement();
            }

            targetObject.transform.SetParent(transform);

            // 타겟을 들고 화면 위로 올라감
            while (transform.position.y < Camera.main.orthographicSize + 5f)
            {
                transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
                yield return null;
            }
        }

        isGrabbing = false;
    }

    private void ReleaseTargetAndReturnToTop()
    {
        // 현재 타겟이 있으면 Die 메서드를 호출하여 비활성화
        if (targetObject != null)
        {
            targetObject.transform.SetParent(null);

            if (targetObject.TryGetComponent(out ChickenObject chicken))
            {
                chicken.Die();
            }
            else
            {
                Destroy(targetObject);
            }

            targetObject = null;
        }

        // 기존 코루틴을 중지하고 최상단으로 돌아간 뒤 새로운 위치로 이동 후 재시작
        if (grabCoroutine != null)
        {
            StopCoroutine(grabCoroutine);
        }

        grabCoroutine = StartCoroutine(ReturnToTopAndReset());
    }

    private IEnumerator ReturnToTopAndReset()
    {
        // 화면 최상단까지 이동
        while (transform.position.y < Camera.main.orthographicSize + 5f)
        {
            transform.position += Vector3.up * ascendSpeed * Time.deltaTime;
            yield return null;
        }

        // 새로운 x 위치를 랜덤하게 설정
        float newX = Random.Range(-xRange, xRange);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        isGrabbing = false;

        // 최상단에 도달한 후 새로운 타겟 잡기 시작
        grabCoroutine = StartCoroutine(GrabTarget());
    }

    private void FindClosestTarget()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestTarget = null;

        // 모든 ChickenObject와 PlayerObject 검색
        ChickenObject[] chickens = FindObjectsOfType<ChickenObject>();
        PlayerObject[] players = FindObjectsOfType<PlayerObject>();

        foreach (var chicken in chickens)
        {
            float distance = Vector2.Distance(transform.position, chicken.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = chicken.gameObject;
            }
        }

        foreach (var player in players)
        {
            float distance = Vector2.Distance(transform.position, player.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestTarget = player.gameObject;
            }
        }

        targetObject = closestTarget;
    }
}
