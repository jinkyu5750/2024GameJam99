using UnityEngine;

public class Player_Movement : Player_State
{
    private float h, v;
    [SerializeField] private bool isSit;
    [SerializeField] private bool isDash;
    private void Update()
    {

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");

        if (h > 0) sr.flipX = true;
        else if (h < 0) sr.flipX = false;

        if (h != 0 || v != 0)
            ani.SetBool("Move", true);
        else if (h == 0 && v == 0)
            ani.SetBool("Move", false);

        if (isSit == false)
        {
            transform.position += new Vector3(h, v, 0) * moveSpeed;
        }
        if (Input.GetKeyDown(KeyCode.Space) && isSit == false && isDash == false)
        {
            isSit = true;
            ani.SetTrigger("Sit");
        }

        if (Input.GetKeyDown(KeyCode.D) && isSit == false && isDash == false)
        {
            isDash = true;
            ani.SetTrigger("Dash");
            rig.AddForce( rig.velocity.normalized * 10, ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate()
    {
      
    }

    public void OffAttrs(string Attr)
    {
        switch (Attr)
        {
            case "Sit":
                isSit = false;
                break;
            case "Dash":
                isDash = false;
                break;
        }
    }
}
