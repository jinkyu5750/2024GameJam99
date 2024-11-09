using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private Animator animator;
    private float horizontalInput;
    private float verticalInput;
    private bool isSitting = false; 
    private bool isAttacking = false; 

    public bool IsSitting()
    {
        return isSitting;
    }

    void Start()
    {
     
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
        if (!isAttacking)
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
        }

        // Check if the 'C' key is held down to sit
        if (Input.GetKey(KeyCode.C))
        {
            if (!isSitting) // Only trigger the sit action if not already sitting
            {
                isSitting = true; // Set sitting state to true
                animator.SetBool("IsSitting", true); // Set IsSitting to true in Animator
                Debug.Log("Sitting down");
            }
        }
        else
        {
            if (isSitting) // Stop sitting when C is released
            {
                isSitting = false; // Set sitting state to false
                animator.SetBool("IsSitting", false); // Set IsSitting to false in Animator
                Debug.Log("Standing up");
            }
        }

        // Check if the Control key is pressed for the attack motion
        if (Input.GetKeyDown(KeyCode.LeftControl) && !isAttacking)
        {
            TriggerAttack();
        }
    }

    void TriggerAttack()
    {
        // Set the attack trigger in the Animator to play the attack animation
        animator.SetTrigger("AttackTrigger");
        isAttacking = true; // Set isAttacking to true to stop movement
    }

    // This method can be called from an Animation Event at the end of the attack animation
    public void EndAttack()
    {
        isAttacking = false; // Reset isAttacking to false to allow movement again
        animator.ResetTrigger("AttackTrigger"); // Reset the trigger after the attack
    }
}