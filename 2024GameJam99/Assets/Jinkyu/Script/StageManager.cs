using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    [Header("�������� ������")]
    [SerializeField] private GameObject[] stage = new GameObject[3];
    public Canvas uiCanvas, inGameCanvas;


    public IEnumerator OffUiCanvas(float duration)
    {
        yield return new WaitForSeconds(duration);
        uiCanvas.gameObject.SetActive(false);
        inGameCanvas.gameObject.SetActive(true);
    }

    public IEnumerator OnUiCanvas(float duration)
    {
        yield return new WaitForSeconds(duration);
        uiCanvas.gameObject.SetActive(true);
        inGameCanvas.gameObject.SetActive(false);
    }

    public void InstantiateMap(int idx)
    {
        Instantiate(stage[idx],Vector3.zero,Quaternion.identity);
    }

}
