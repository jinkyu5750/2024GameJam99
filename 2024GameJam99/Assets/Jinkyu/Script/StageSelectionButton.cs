using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using EasyTransition;
public class StageSelectionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public AudioSource audioSource;
    public TransitionSettings t;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    void Update()
    {

    }


    public void ClickStageButton()
    {

        audioSource.clip = SoundManager.instance.buttonClick;
        audioSource.Play();

        TransitionManager.Instance().runningTransition = false;
        TransitionManager.Instance().Transition(t, 0.2f);

        int stageNum = int.Parse(transform.name[11].ToString());
        StartCoroutine(StageManager.instance.OffUiCanvas(1.2f));
        StageManager.instance.InstantiateMap(stageNum - 1);
        GameManager.Instance.clearTime = 0f;
        GameManager.Instance.isGameStart = true;
        
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
