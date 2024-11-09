using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    public Image nightOverlayPanel; // 밤을 표현하는 오버레이 패널
    private float dayDuration = 10f; // 낮 지속 시간
    private float nightDuration = 3f; // 밤 지속 시간
    private bool isNight = false; // 현재 밤인지 여부


    private int currentStageIndex = 0;
    private int currentScore = 0;
    
    public TextMeshProUGUI currentScoreText;


    public void UpdateUI(int infectedChickenCount)
    {

    }

    public void GameEnd()
    {
        //player가 죽었을때, lose
        
        //Player가 묙표 달성했을때, win
        
    }
    
    private void Start()
    {

        nightOverlayPanel = GameObject.Find("NightImage").GetComponent<Image>();
        // 오버레이 패널이 설정되어 있으면 낮-밤 주기를 시작
        if (nightOverlayPanel != null)
        {
            nightOverlayPanel.gameObject.SetActive(true);
            StartCoroutine(DayNightCycle());
        }
    }

    public void Init()
    {
        
    }
    
    // 낮-밤 주기 코루틴
    private IEnumerator DayNightCycle()
    {
        while (true)
        {
            // 밤으로 전환 (오버레이 패널 페이드 인)
            yield return StartCoroutine(FadeOverlay(0f, 0.7f, 1f));
            isNight = true;
            yield return new WaitForSeconds(nightDuration);

            // 낮으로 전환 (오버레이 패널 페이드 아웃)
            yield return StartCoroutine(FadeOverlay(0.7f, 0f, 1f));
            isNight = false;
            yield return new WaitForSeconds(dayDuration);
        }
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