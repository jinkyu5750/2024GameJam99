using System.Collections;
using System.Collections.Generic;
using System.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public bool isCatchable;
    
    private Image nightOverlayPanel; // 밤을 표현하는 오버레이 패널
    private float dayDuration = 5f; // 낮 지속 시간
    private float nightDuration = 2f; // 밤 지속 시간
    private bool isNight = false; // 현재 밤인지 여부

    private Camera camera;

    private int currentStageIndex = 0;
    private int currentScore = 0;

    private GameManager gm;
    private Player_Movement playerMove;

    public Hand hand;
    private GameObject player;
    
    private void Start()
    {
        camera = Camera.main;
        CameraFollow cf = camera.gameObject.GetComponent<CameraFollow>();

        player = GameObject.Find("Player");
        playerMove = player.GetComponent<Player_Movement>();
        cf.SetFollow(player.transform);

        nightOverlayPanel = GameObject.Find("NightImage").GetComponent<Image>();
        // 오버레이 패널이 설정되어 있으면 낮-밤 주기를 시작
        if (nightOverlayPanel != null)
        {
            nightOverlayPanel.gameObject.SetActive(true);
            StartCoroutine(DayNightCycle());
        }
        
        gm.UpdateUI();
    }

    public void Init(GameManager gm)
    {
        this.gm = gm;
    }

    public ChickenObject GetOneNotSit()
    {
        ChickenObject chic = gm.GetRandomAliveChickenAtNight();
        
        chic.SetSit(false);
        
        return chic;
    }
    
    // 낮-밤 주기 코루틴
    private IEnumerator DayNightCycle()
    {
        while (true)
        {
            // 낮으로 전환 (오버레이 패널 페이드 아웃)
            yield return StartCoroutine(FadeOverlay(0.7f, 0f, 0.5f));
            isNight = false;
            gm.SetSit(false);
            yield return new WaitForSeconds(dayDuration+Random.Range(-1.0f, 1.0f));
            
            // 밤으로 전환 (오버레이 패널 페이드 인)
            yield return StartCoroutine(FadeOverlay(0f, 0.7f, 0.5f));
            isNight = true;
            gm.SetSit(true);
            StartCoroutine(CatchOne());
            yield return new WaitForSeconds(nightDuration+Random.Range(0.0f, 2.0f));


        }
    }

    private IEnumerator CatchOne()
    {
        gm.UpdateUI();
        yield return new WaitForSeconds(0.5f);
        ChickenObject co = GetOneNotSit();
        if (isCatchable)
        {
            if (playerMove.isSit == false)
            {
                Debug.Log(playerMove.gameObject.name);
                
                Player_State ps = player.GetComponent<Player_State>();
                ps.HP = 0;
            }
        }
        yield return new WaitForSeconds(1f);
        hand.Init(co.gameObject);
        
        yield return null;
    }

    // 오버레이 패널의 알파 값을 부드럽게 전환하는 코루틴
    private IEnumerator FadeOverlay(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0f;
        Color overlayColor = nightOverlayPanel.color;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            overlayColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            nightOverlayPanel.color = overlayColor;
            yield return null;
        }

        overlayColor.a = endAlpha;
        nightOverlayPanel.color = overlayColor;
    }

    // 현재 밤인지 상태를 반환
    public bool IsNight()
    {
        return isNight;
    }
}