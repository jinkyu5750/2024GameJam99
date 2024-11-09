using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasOverlayController : MonoBehaviour
{
    public GameObject gasOverlayObject;  // Assign the GameObject here
    public float minGasDensity = 0.3f;   // Minimum gas density level (30% opacity)
    public float maxGasDensity = 0.7f;   // Maximum gas density level (70% opacity)
    public float transitionDuration = 2f; // Duration of gas filling and dissipating
    public float minWaitTime = 2f;       // Minimum time before next gas release
    public float maxWaitTime = 10f;      // Maximum time before next gas release

    private SpriteRenderer gasRenderer;
    private bool isReleasingGas = false;

    private void Start()
    {
        // Ensure the GameObject has a SpriteRenderer component
        if (gasOverlayObject != null)
        {
            gasRenderer = gasOverlayObject.GetComponent<SpriteRenderer>();
            if (gasRenderer != null)
            {
                // Start with no gas by setting alpha to 0
                Color color = gasRenderer.color;
                color.a = 0f;
                gasRenderer.color = color;

                // Start the random gas routine
                StartCoroutine(RandomGasRoutine());
            }
            else
            {
                Debug.LogError("No SpriteRenderer found on the gas overlay object.");
            }
        }
    }

    private IEnumerator RandomGasRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(waitTime);

            if (!isReleasingGas)
            {
                float targetDensity = Random.Range(minGasDensity, maxGasDensity); // Random gas density
                yield return StartCoroutine(FadeToGasDensity(targetDensity));
            }
        }
    }

    private IEnumerator FadeToGasDensity(float targetDensity)
    {
        isReleasingGas = true;
        float startDensity = gasRenderer.color.a;
        float elapsedTime = 0f;

        // Fade to target gas density
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newDensity = Mathf.Lerp(startDensity, targetDensity, elapsedTime / transitionDuration);
            gasRenderer.color = new Color(gasRenderer.color.r, gasRenderer.color.g, gasRenderer.color.b, newDensity);
            yield return null;
        }

        yield return new WaitForSeconds(transitionDuration);

        // Fade back to transparency (gas dissipates)
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float newDensity = Mathf.Lerp(targetDensity, 0f, elapsedTime / transitionDuration);
            gasRenderer.color = new Color(gasRenderer.color.r, gasRenderer.color.g, gasRenderer.color.b, newDensity);
            yield return null;
        }

        isReleasingGas = false;
    }
}