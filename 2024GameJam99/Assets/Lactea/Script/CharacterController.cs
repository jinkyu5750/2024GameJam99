using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;
    private float horizontalInput;
    private float verticalInput;
    private bool isSitting = false; // Track if the character is sitting
    
    public bool IsSitting(){
        return isSitting;
    }
    void Start()
    {
        // Get the Animator component attached to the character
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Get player input for movement (left, right, up, down)
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // Check if the player is pressing any direction key (left, right, up, down)
        bool isMoving = horizontalInput != 0 || verticalInput != 0;

        // Set the IsMoving parameter in Animator based on movement input
        animator.SetBool("IsMoving", isMoving);

        // Move the character if the keys are pressed
        if (isMoving)
        {
            Vector3 movement = new Vector3(horizontalInput, verticalInput, 0) * Time.deltaTime * 5f; // Adjust speed as needed
            transform.Translate(movement);
        }

        // Check if the 'C' key is held down to sit
        if (Input.GetKey(KeyCode.C)) // Check if C is being held down
        {
            if (!isSitting) // Only trigger the sit action if not already sitting
            {
                isSitting = true; // Set sitting state to true
                animator.SetBool("IsSitting", true); // Set IsSitting to true in Animator
                Debug.Log("Sitting down");
            }
        }
        else // When "C" key is released
        {
            if (isSitting) // Stop sitting when C is released
            {
                isSitting = false; // Set sitting state to false
                animator.SetBool("IsSitting", false); // Set IsSitting to false in Animator
                Debug.Log("Standing up");
            }
        }
    }
}
