using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenDarkener : MonoBehaviour
{
    public Image darkOverlay;       // Assign the dark overlay image here
    public float minDarkness = 0.3f; // Minimum darkness level (30% opacity)
    public float maxDarkness = 0.7f; // Maximum darkness level (70% opacity)
    public float transitionDuration = 2f; // Duration of darkening and lightening effects
    public float minWaitTime = 2f;   // Minimum time before next darkening
    public float maxWaitTime = 10f;  // Maximum time before next darkening

    private bool isDarkening = false;

    private void Start()
    {
        if (darkOverlay != null)
        {
            darkOverlay.color = new Color(darkOverlay.color.r, darkOverlay.color.g, darkOverlay.color.b, 0f);
            StartCoroutine(RandomDarkenRoutine());
        }
    }

    private System.Collections.IEnumerator RandomDarkenRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);
            
            if (!isDarkening)
            {
                float targetAlpha = Random.Range(minDarkness, maxDarkness);
                yield return StartCoroutine(FadeToDarkness(targetAlpha));
            }
        }
    }

    private System.Collections.IEnumerator FadeToDarkness(float targetAlpha)
    {
        isDarkening = true;
        float startAlpha = darkOverlay.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / transitionDuration);
            darkOverlay.color = new Color(darkOverlay.color.r, darkOverlay.color.g, darkOverlay.color.b, newAlpha);
            yield return null;
        }

        yield return new WaitForSeconds(transitionDuration);

        // Fade back to transparency
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(targetAlpha, 0f, elapsedTime / transitionDuration);
            darkOverlay.color = new Color(darkOverlay.color.r, darkOverlay.color.g, darkOverlay.color.b, newAlpha);
            yield return null;
        }

        isDarkening = false;
    }
}