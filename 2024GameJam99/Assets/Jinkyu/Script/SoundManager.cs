using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("BGM")]
    [SerializeField] public AudioClip openingChicken;
    [SerializeField] public AudioClip startBGM;
    [SerializeField] public AudioClip stageSellectionBGM;
    [SerializeField] public AudioClip[] inStageBGM = new AudioClip[3];
    [Header("UI")]
    [SerializeField] public AudioClip buttonClick;
    [SerializeField] public AudioClip esc;
    [SerializeField] public AudioClip clear;

    void Awake()
    {

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

    }

}
