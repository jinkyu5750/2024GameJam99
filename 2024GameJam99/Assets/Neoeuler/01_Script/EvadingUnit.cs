
using UnityEngine;

public class EvadingUnit : MonoBehaviour
{
    public Transform player; // 플레이어 객체의 Transform
    public float evadeSpeed = 5f; // 유닛의 이동 속도
    public float safeDistance = 10f; // 플레이어로부터 안전 거리

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        EvadePlayer();
    }

    void EvadePlayer()
    {
        // 플레이어와 유닛 사이의 거리 계산
        float distance = Vector2.Distance(transform.position, player.position);

        // 플레이어가 안전 거리 안에 들어왔는지 확인
        if (distance < safeDistance)
        {
            // 플레이어의 방향을 벗어나는 방향 계산
            Vector2 evadeDirection = (transform.position - player.position).normalized;

            // evadeSpeed에 따라 도망가는 방향으로 이동
            rb.MovePosition(rb.position + evadeDirection * evadeSpeed * Time.deltaTime);
        }
        else
        {
            // 안전 거리 밖이라면 움직임을 멈춥니다.
            rb.velocity = Vector2.zero;
        }
    }
}

