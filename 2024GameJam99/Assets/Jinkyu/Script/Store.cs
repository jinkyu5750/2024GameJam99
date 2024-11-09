using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Store : MonoBehaviour
{
    public Ease ease;
    [SerializeField] private GameObject stagePanel;
    [SerializeField] private GameObject storePanel;

    [SerializeField] private bool isClick = false;

    public void ClickStore()
    {
        if (isClick == false)
        {
            isClick = true;
            stagePanel.transform.DOMoveX(-1800, 1f).SetEase(ease);
            storePanel.transform.DOMoveX(-900f, 1f).SetEase(ease).OnComplete(() =>
            {
                
            });
        }
        else if( isClick == true)
        {
            isClick = false;
            stagePanel.transform.DOMoveX(0, 1f).SetEase(ease);
            storePanel.transform.DOMoveX(900f, 1f).SetEase(ease);
        }
    }
}
