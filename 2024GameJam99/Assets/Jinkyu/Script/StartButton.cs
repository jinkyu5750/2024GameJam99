using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using EasyTransition;


public class StartButton : MonoBehaviour
{
    public TransitionSettings transition;
    [SerializeField] private Ease ease;
    public void ClickStartButton()
    {
       
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector2(1.15f, 1.15f), 0.1f).SetEase(ease));
        seq.Append(transform.DOScale(new Vector2(0.95f, 0.95f), 0.1f).SetEase(ease));
        seq.Play().OnComplete(() =>
        {
            TransitionManager.Instance().Transition("StageMenu", transition, 0.5f);
           // SceneManager.LoadScene("StageMenu");
        }); ; 

    }
}
