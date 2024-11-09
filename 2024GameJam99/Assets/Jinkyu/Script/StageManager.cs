using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;


    [Header("스테이지 프리팹")]
    [SerializeField] private GameObject[] stage = new GameObject[3];
    [SerializeField] private bool[] stageClear = { false, false, false }; // 클리어시 true로, 셋다 true면 엔딩 // 클리어했다는걸 표시할게필요함
    [SerializeField] private int currentStageIdx = -1; // -1인 경우는 스테이지선택씬,시작씬 , 사실 얘 있어야되는진 모르겠음아직
    public Canvas uiCanvas, inGameCanvas;
    public GameObject currentStage;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

    public IEnumerator OffUiCanvas(float duration)
    {
        yield return new WaitForSeconds(duration);
        uiCanvas.gameObject.SetActive(false);
        inGameCanvas.gameObject.SetActive(true);
    }

    public IEnumerator OnUiCanvas(float duration)
    {
        yield return new WaitForSeconds(duration);

        if (inGameCanvas.transform.GetChild(2).gameObject.activeSelf == true) // ㅈ~~~ㄴ 레전드 코드 UIManager좀해놓지 ㅋ
        {

            inGameCanvas.transform.GetChild(2).transform.GetChild(0).transform.GetChild(3).transform.localScale = Vector3.zero;
            inGameCanvas.transform.GetChild(2).gameObject.SetActive(false);

        }
        uiCanvas.gameObject.SetActive(true);
        inGameCanvas.gameObject.SetActive(false);
    }

    public void InstantiateMap(int idx)
    {
        currentStageIdx = idx;
        GameObject go;
        if (stageClear[idx] == false)
        {
            go = Instantiate(stage[idx], Vector3.zero, Quaternion.identity);
            currentStage = go;
        }
    }

}
