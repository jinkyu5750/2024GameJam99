using DG.Tweening;
using EasyTransition;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ESC : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    //임시
    public GameObject p;

    [SerializeField] private GameObject escPanel;

    public Ease ease;
    public TransitionSettings t;


    private void Start()
    {
        escPanel.transform.localScale = new Vector3(1, 0, 1);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameManager.Instance.isGameStart == true)
        {
            
            escPanel.SetActive(true);
            escPanel.transform.DOScaleY(1, 0.15f).SetEase(ease);
        }
    }

    public void Resume()
    {

        escPanel.transform.DOScaleY(0, 0.15f).SetEase(ease).OnComplete(() =>
        {
            escPanel.SetActive(false);
        });

    }

    public void Option()
    {
        p.SetActive(true);
        Debug.Log("없는기능 . . . . . .");
    }

    public void Exit()
    {

        escPanel.SetActive(false);

        TransitionManager.Instance().runningTransition = false;
        TransitionManager.Instance().Transition(t, 0.1f);

        Invoke("DestroyStage",1f);
        StartCoroutine(StageManager.instance.OnUiCanvas(1f));

        GameManager.Instance.isGameStart = false;
    }

    void DestroyStage()
    {
        Destroy(StageManager.instance.currentStage);
        StageManager.instance.currentStage = null;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1.1f, 1.1f), 0.2f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(new Vector2(1f, 1f), 0.2f);
    }
}
