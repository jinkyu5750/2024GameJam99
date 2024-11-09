using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // 따라갈 대상 (예: PlayerObject)
    public float smoothSpeed = 0.125f; // 카메라 이동 속도
    public Vector3 offset; // 카메라와 대상 간 거리 오프셋
    

    void LateUpdate()
    {
        if (target == null)
            return;

        // 목표 위치 계산 (타겟 위치 + 오프셋, z축은 현재 카메라 z위치로 고정)
        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, transform.position.z);

        // 현재 위치에서 목표 위치로 부드럽게 이동
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // 카메라 위치 설정
        transform.position = smoothedPosition;
    }

    public void SetFollow(Transform trans)
    {
        target = trans;
    }
}