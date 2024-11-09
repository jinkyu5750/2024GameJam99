using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using EasyTransition;


public class StartButton : MonoBehaviour
{
    public AudioSource audioSource;
    public TransitionSettings transition;
    [SerializeField] private Ease ease;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void ClickStartButton()
    {
        audioSource.clip = SoundManager.instance.buttonClick;
        audioSource.Play();

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(new Vector2(1.15f, 1.15f), 0.1f).SetEase(ease));
        seq.Append(transform.DOScale(new Vector2(0.95f, 0.95f), 0.1f).SetEase(ease));
        seq.Play().OnComplete(() =>
        {
            TransitionManager.Instance().Transition("StageMenu", transition, 0.2f);
           // SceneManager.LoadScene("StageMenu");
        }); ; 

    }
}
