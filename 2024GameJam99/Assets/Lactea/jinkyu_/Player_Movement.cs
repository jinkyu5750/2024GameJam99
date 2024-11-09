using UnityEngine;

public class Player_Movement : Player_State
{
    private float h, v;
    [SerializeField] private bool isSit;
    [SerializeField] private bool isDash;
    private float dashSpeed = 0.8f;
    private float dashCur=0, dashCool = 5f;
    private void Update()
    {

        if (dashCur >= 0)
        {
            dashCur -= Time.deltaTime;
        }
        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h < 0)
            sr.flipX = false;
        else if (h > 0)
            sr.flipX = true;

        if (h != 0 || v != 0)
            ani.SetBool("Move", true);
        else if (h == 0 && v == 0)
            ani.SetBool("Move", false);

        if (Input.GetKeyDown(KeyCode.Space) && isSit == false && isDash == false)
        {
            isSit = true;
            ani.SetTrigger("Sit");
            Invoke("OffIsSit", 0.7f);
        }

        if (Input.GetKeyDown(KeyCode.C) && dashCur <= 0 && isSit == false && isDash == false)
        {
            dashCur = dashCool;
            isDash = true;
            ani.SetTrigger("Dash");
            moveSpeed = dashSpeed;
            Invoke("SetMoveSpeed", 1.5f);
        }
    }


    public void SetMoveSpeed()
    {
        moveSpeed = 0.3f;
    }
    private void FixedUpdate()
    {

        if (isSit == false && isDash == false)
        {
            rig.position += new Vector2(h, v) * moveSpeed;
        }
    }


    public void OffIsSit()
    {
        isSit = false;
    }
    public void OffIsDash()
    {
        isDash = false;
    }
    
}
