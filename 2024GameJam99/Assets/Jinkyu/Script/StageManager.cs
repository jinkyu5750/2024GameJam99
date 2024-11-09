using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;


    [Header("�������� ������")]
    [SerializeField] private GameObject[] stage = new GameObject[3];
    [SerializeField] private bool[] stageClear = { false, false, false }; // Ŭ����� true��, �´� true�� ���� // Ŭ�����ߴٴ°� ǥ���Ұ��ʿ���
    [SerializeField] private int currentStageIdx = -1; // -1�� ���� �����������þ�,���۾� , ��� �� �־�ߵǴ��� �𸣰�������
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

        if (inGameCanvas.transform.GetChild(2).gameObject.activeSelf == true) // ��~~~�� ������ �ڵ� UIManager���س��� ��
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
