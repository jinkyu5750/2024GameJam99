using System.Collections;
using UnityEngine;

public class MainBGM : MonoBehaviour
{

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(PlayBGM());
    }

    IEnumerator PlayBGM()
    {
      

        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            if (!audioSource.isPlaying)
            {
                audioSource.clip = SoundManager.instance.startBGM;
                audioSource.Play();
                audioSource.loop = true;
            }
        }
    }

}
