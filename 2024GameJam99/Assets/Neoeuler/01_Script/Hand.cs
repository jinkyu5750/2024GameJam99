using System.Collections;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public float liftHeight = 2f; // 오브젝트를 들어 올릴 높이
    public float liftDuration = 1f; // 들어 올리는 데 걸리는 시간
    private GameObject targetObject; // 들어 올릴 타겟 오브젝트

    public void Init(GameObject target)
    {
        gameObject.SetActive(true);
        targetObject = target;

        // 타겟 오브젝트를 완전히 정지
        targetObject.GetComponent<ChickenObject>()?.CompletelyStop();

        // Hand 오브젝트 위치를 타겟 오브젝트 위치로 이동
        transform.position = targetObject.transform.position;
        
        // 들어 올리는 코루틴 시작
        StartCoroutine(PickUpCoroutine());
    }

    private IEnumerator PickUpCoroutine()
    {
        Vector3 startPosition = targetObject.transform.position;
        Vector3 endPosition = startPosition + Vector3.up * liftHeight;
        float elapsedTime = 0f;

        // 타겟 오브젝트와 Hand 오브젝트를 함께 위로 들어 올리기
        while (elapsedTime < liftDuration)
        {
            Vector3 currentPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / liftDuration);
            targetObject.transform.position = currentPosition;
            transform.position = currentPosition; // Hand도 함께 이동
            
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 설정
        targetObject.transform.position = endPosition;
        transform.position = endPosition;


        
        // 일정 시간 후 Hand와 타겟 오브젝트를 비활성화
        targetObject.SetActive(false);
        gameObject.SetActive(false);
        
        targetObject.GetComponent<ChickenObject>()?.Die();
    }
}