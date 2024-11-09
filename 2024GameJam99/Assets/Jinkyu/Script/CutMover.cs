using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutMover : MonoBehaviour
{
    public Ease ease;
    public GameObject[] cut = new GameObject[3];
    public AudioSource audioSource;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartCutScene());
    }

    IEnumerator StartCutScene()
    {
        yield return new WaitForSeconds(1f);

        Sequence seq = DOTween.Sequence();

        audioSource.Play();
        seq.Append(cut[0].transform.DOMoveY(0, 2f).SetEase(ease).OnComplete(() =>
        {
            audioSource.Play();
        }));
        seq.Append(cut[1].transform.DOMoveY(0, 2f).SetEase(ease).OnComplete(() =>
        {
            audioSource.Play();
        }));
        seq.Append(cut[2].transform.DOMoveY(0, 2f).SetEase(ease));
        seq.Play();
    }
}
