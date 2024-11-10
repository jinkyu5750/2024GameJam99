using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelectionBGM : MonoBehaviour
{
    AudioSource audioSource;
    AudioClip currentClip;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // ó�� ������ �� �⺻ BGM ����
        currentClip = SoundManager.instance.stageSellectionBGM;
        audioSource.clip = currentClip;
        audioSource.Play();
    }

    void Update()
    {
        AudioClip newClip;

        if (StageManager.instance.currentStage != null)
        {
            newClip = SoundManager.instance.inStageBGM[0];
        }
        else
        {
            newClip = SoundManager.instance.stageSellectionBGM;
        }

        // Ŭ���� ����Ǿ��� ���� ���
        if (newClip != currentClip)
        {
            currentClip = newClip;
            audioSource.clip = currentClip;
            audioSource.Play();
        }
    }
}
