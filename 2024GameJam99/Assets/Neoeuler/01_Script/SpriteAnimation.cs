using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class SpriteAnimation : MonoBehaviour
{
    public SpriteRenderer spriteRenderer; // SpriteRenderer 참조
    public Sprite[] walkSprites; // 애니메이션에 사용할 Sprite 배열
    public Sprite[] sitSprites;
    public Sprite[] coughSprites;
    public float frameRate = 0.1f; // 각 프레임의 재생 시간 (초)

    private int currentFrame = 0; // 현재 프레임 인덱스
    private Coroutine animationCoroutine;

    private float tempDuration;
    private Sprite[] spriteHandle;
    
    public ObjectState state;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (walkSprites.Length > 0)
        {
            // 애니메이션 코루틴을 시작하여 반복 재생
            animationCoroutine = StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        while (true)
        {
            // 현재 프레임을 SpriteRenderer에 설정
            spriteRenderer.sprite = walkSprites[currentFrame];

            // 다음 프레임으로 이동 (반복 순환)
            currentFrame = (currentFrame + 1) % walkSprites.Length;
            
            // 각 프레임의 재생 시간만큼 대기
            yield return new WaitForSeconds(frameRate);
        }
    }

    // 애니메이션 시작 메서드 (외부에서 호출 가능)
    public void StartAnimation()
    {
        if (animationCoroutine == null && walkSprites.Length > 0)
        {
            animationCoroutine = StartCoroutine(PlayAnimation());
        }
    }

    // 애니메이션 중지 메서드 (외부에서 호출 가능)
    public void StopAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }
}