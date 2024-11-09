using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 패턴으로 접근 가능하게 설정
    private List<ChickenObject> chickens = new List<ChickenObject>(); // 모든 치킨을 관리하는 리스트

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 치킨 등록
    public void RegisterChicken(ChickenObject chicken)
    {
        if (!chickens.Contains(chicken))
        {
            chickens.Add(chicken);
        }
    }

    // 치킨 제거
    public void UnregisterChicken(ChickenObject chicken)
    {
        if (chickens.Contains(chicken))
        {
            chickens.Remove(chicken);
        }
    }
    
    public List<ChickenObject> GetAllChickens()
    {
        return chickens;
    }

    // 가장 가까운 비감염 치킨 찾기
    public ChickenObject FindClosestUninfectedChicken(Vector2 position)
    {
        ChickenObject closestChicken = null;
        float closestDistance = Mathf.Infinity;

        foreach (var chicken in chickens)
        {
            if (!chicken.IsInfected) // 비감염 치킨만 탐색
            {
                float distance = Vector2.Distance(position, chicken.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestChicken = chicken;
                }
            }
        }
        return closestChicken;
    }
    
    // 두 번째로 가까운 비감염 치킨 찾기 (첫 번째 감염된 대상을 제외)
    public ChickenObject FindSecondClosestUninfectedChicken(Vector2 position, ChickenObject firstTarget)
    {
        ChickenObject secondClosestChicken = null;
        float secondClosestDistance = Mathf.Infinity;

        foreach (var chicken in chickens)
        {
            if (!chicken.IsInfected && chicken != firstTarget) // 비감염 치킨이면서 첫 번째 대상이 아닌 치킨만 탐색
            {
                float distance = Vector2.Distance(position, chicken.transform.position);
                if (distance < secondClosestDistance)
                {
                    secondClosestDistance = distance;
                    secondClosestChicken = chicken;
                }
            }
        }
        return secondClosestChicken;
    }
    
    // 현재 감염된 치킨 수를 반환
    public int GetInfectedChickenCount()
    {
        int infectedCount = 0;
        foreach (var chicken in chickens)
        {
            if (chicken.IsInfected) infectedCount++;
        }
        return infectedCount;
    }

    // 현재 살아있는 치킨 수를 반환
    public int GetAliveChickenCount()
    {
        int aliveCount = 0;
        foreach (var chicken in chickens)
        {
            if (chicken.gameObject.activeSelf) aliveCount++;
        }
        return aliveCount;
    }
    
}