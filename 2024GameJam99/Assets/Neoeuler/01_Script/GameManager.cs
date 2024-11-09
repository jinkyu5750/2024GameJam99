using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // 싱글톤 패턴으로 접근 가능하게 설정
    [SerializeField]private List<ChickenObject> chickens = new List<ChickenObject>(); // 모든 치킨을 관리하는 리스트

    public GameObject pickHandle;

    public bool isGameStart = false;
    public float clearTime = 0; // 게임 진행 시간

    public TextMeshProUGUI scoreDisplay;
    public TextMeshProUGUI elapsedTime;
    
    [SerializeField] private GameScene gameSceneHandle;
    [SerializeField] private Player_State playerState;
    [SerializeField] private Clear clear;

    private int stageNumber = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 다른 씬 이동 시 삭제되지 않도록 설정
        }
        else
        {
            Destroy(gameObject); // 싱글톤이므로 추가 인스턴스를 방지
        }
    }

    public void SetStage(int index)
    {
        chickens.Clear();
        clearTime = 0;
        stageNumber = index;
        
        scoreDisplay.gameObject.SetActive(true);
        int currentScore = GetInfectedChickenCount();
        int currentMax = GetAliveChickenCount();
        scoreDisplay.text = $"{currentScore} / {currentMax}";
    }
    
    private void Update()
    {
        if (StageManager.instance != null && StageManager.instance.currentStage != null && isGameStart)
        {
            clearTime += Time.deltaTime; // 클리어 타임 누적
            if (gameSceneHandle == null)
            {
                gameSceneHandle = GameObject.Find("GameScene").GetComponent<GameScene>();
                gameSceneHandle.Init(this);
                playerState = GameObject.Find("Player").GetComponent<Player_State>();
                
            }

            UpdateElapsedTimeDisplay();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            //SetSit(true);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            //SetSit(false);
        }


    }

    private void UpdateElapsedTimeDisplay()
    {
        int minutes = Mathf.FloorToInt(clearTime / 60); // 분 계산
        int seconds = Mathf.FloorToInt(clearTime % 60); // 초 계산

        // "분:초" 형식으로 텍스트 업데이트
        elapsedTime.text = $"{minutes:00}:{seconds:00}";
    }
    
    // 치킨 등록
    public void RegisterChicken(ChickenObject chicken)
    {
        if (!chickens.Contains(chicken))
        {
            chickens.Add(chicken);
            chicken.Init(UpdateUI);
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

    public Transform GetRandomChickenPick()
    {
        int rand = Random.Range(0, chickens.Count);
        return chickens[rand].transform;
    }

    public void SetSit(bool flag)
    {
        foreach (var chicken in chickens)
        {
            if (chicken.gameObject.activeSelf)
            {
                chicken.SetSit(flag);
            }
        }
    }
    
    public ChickenObject GetRandomAliveChickenAtNight()
    {
        List<ChickenObject> aliveChickens = new List<ChickenObject>();

        // 살아있는 chicken을 리스트에 추가
        foreach (var chicken in chickens)
        {
            if (chicken.gameObject.activeSelf)
            {
                aliveChickens.Add(chicken);
            }
        }

        // 살아있는 chicken이 없으면 null 반환
        if (aliveChickens.Count == 0)
        {
            return null;
        }

        // 무작위로 살아있는 chicken 선택
        int randomIndex = Random.Range(0, aliveChickens.Count);
        return aliveChickens[randomIndex];
    }


    public void UnsetSitRandomly()
    {
        var rand = Random.Range(0, chickens.Count);
        
        chickens[rand].SetSit(false);
    }
    
    public void UpdateUI()
    {
        
        scoreDisplay.gameObject.SetActive(true);
        int currentScore = GetInfectedChickenCount();
        int currentMax = GetAliveChickenCount();
        scoreDisplay.text = $"{currentScore} / {currentMax}";

        if (currentScore >= currentMax)
        {
            GameWin();
        }
    }

    public void GameWin()
    {
        if (clear.gameObject.activeSelf)
        {
            return;
        }
        //win
        Debug.Log("Game Result call");
        clear.InitClearTime(clearTime);
        clear.InitRemainedChickeNum(GetAliveChickenCount());
        foreach (var chicken in chickens)
        {
            chicken.CompletelyStop();
        }
        
        clear.gameObject.SetActive(true);

        //StageManager.instance.stageClear[stageNumber] = true;
    }
    
}
