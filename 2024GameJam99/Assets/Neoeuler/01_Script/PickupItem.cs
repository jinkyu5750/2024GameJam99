using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public int scoreValue = 10; // 획득 시 얻을 점수나 아이템 가치
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 충돌 감지
        if (other.CompareTag("Player"))
        {
            // 플레이어에게 점수나 아이템을 전달하는 함수 호출
            PlayerObject player = other.transform.GetComponent<PlayerObject>();
            if (player != null)
            {
                //player.AddScore(scoreValue); // 점수나 아이템 추가
                Debug.Log("pickup");
            }
            
            // 픽업 아이템 제거
            Destroy(gameObject);
        }
    }
}