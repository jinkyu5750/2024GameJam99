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
        // 처음 시작할 때 기본 BGM 설정
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

        // 클립이 변경되었을 때만 재생
        if (newClip != currentClip)
        {
            currentClip = newClip;
            audioSource.clip = currentClip;
            audioSource.Play();
        }
    }
}
