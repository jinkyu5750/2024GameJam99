using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarFollow : MonoBehaviour
{
    public Transform healthBar; // Assign the health bar's transform in the Inspector
    public Vector3 offset = new Vector3(0, 0.5f, 0); // The offset from the character's position
    public float followSpeed = 3f; // Speed at which the health bar follows the character (smaller value = slower)
    private Camera mainCamera;

    private Vector3 lastPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        lastPosition = transform.position; // Store the initial position of the character
    }

    private void Update()
    {
        if (healthBar != null)
        {
            // Only update if the character's position has changed
            if (transform.position != lastPosition)
            {
                // Calculate the target position based on the character's position + offset
                Vector3 targetPosition = transform.position + offset;

                if (mainCamera != null && healthBar.GetComponentInParent<Canvas>().renderMode == RenderMode.ScreenSpaceCamera)
                {
                    // Canvas is in Screen Space - Camera mode
                    Vector3 screenPosition = mainCamera.WorldToScreenPoint(targetPosition);
                    healthBar.position = Vector3.Lerp(healthBar.position, screenPosition, Time.deltaTime * followSpeed);
                }
                else
                {
                    // Canvas is in World Space mode
                    healthBar.position = Vector3.Lerp(healthBar.position, targetPosition, Time.deltaTime * followSpeed);
                }

                lastPosition = transform.position; // Update the last position
            }
        }
    }
}