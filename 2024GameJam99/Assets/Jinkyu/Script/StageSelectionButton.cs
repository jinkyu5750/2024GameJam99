using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using EasyTransition;
public class StageSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private StageManager SM;
    public TransitionSettings t;

    void Start()
    {
        SM = GameObject.Find("StageManager").GetComponent<StageManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void ClickStageButton()
    {

        TransitionManager.Instance().runningTransition = false;
        TransitionManager.Instance().Transition(t, 0.5f);

        int stageNum = int.Parse(transform.name[11].ToString());
        StartCoroutine(SM.OffUiCanvas(1.5f));
        SM.InstantiateMap(stageNum-1);
        
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
