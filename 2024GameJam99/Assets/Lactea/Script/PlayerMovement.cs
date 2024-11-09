using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;  // 이동 속도 조절
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // SpriteRenderer 컴포넌트를 가져옵니다
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 수평 및 수직 입력 받기 (GetAxisRaw로 부드러운 입력을 없앰)
        float moveDirectionX = Input.GetAxisRaw("Horizontal");  // 왼쪽, 오른쪽
        float moveDirectionY = Input.GetAxisRaw("Vertical");    // 위, 아래

        // 수직 방향 속도 절반으로 줄이기
        moveDirectionY /= 2f;  // Y축 속도를 절반으로 줄임

        // 수평 및 수직 이동을 빠르게 적용
        Vector3 newPosition = transform.position;
        newPosition.x += moveDirectionX * speed * Time.deltaTime; // 수평 이동
        newPosition.y += moveDirectionY * speed * Time.deltaTime; // 수직 이동

        // 캐릭터 방향 설정 (좌우 이동 시에만 변경)
        if (moveDirectionX > 0)
        {
            spriteRenderer.flipX = true;  // 오른쪽을 바라봄
        }
        else if (moveDirectionX < 0)
        {
            spriteRenderer.flipX = false;  // 왼쪽을 바라봄
        }

        // 위치를 이동 (단일 프레임에 한 번만)
        transform.position = newPosition;
    }
}