using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    public Player_State PS;

    public GameObject Gas1_1,Gas1_2;
    public GameObject Gas2_1,Gas2_2;

    public GameObject Gas1Pos;
    public GameObject Gas2Pos;

    public float attack1Col = 3f, attack1Cur;
    public float attack2Col = 5f, attack2Cur;

    private void Start()
    {
        attack1Cur = 0;
        attack2Cur = 0;
    }
    private void Update()
    {

        if (attack1Cur >= 0)
        {
            attack1Cur -= Time.deltaTime;
        }
        if (attack2Cur >= 0)
        {
            attack2Cur -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Z) && attack1Cur <= 0)
        {
            attack1Cur = attack1Col;
            PS.ani.SetTrigger("Attack1");
        }

        if (Input.GetKeyDown(KeyCode.X) && attack2Cur <= 0)
        {
            attack2Cur = attack2Col;
            InstantiatePrefab("Attack2");

        }
    }
    public void InstantiatePrefab(string attack)
    {
        GameObject go,go2;
        switch (attack)
        {

            case "Attack1":
                if(PS.sr.flipX == false)
                go = Instantiate(Gas1_2, Gas1Pos.transform.position, Quaternion.identity);
                else
                    go = Instantiate(Gas1_1, Gas1Pos.transform.position, Quaternion.identity);

                go.transform.parent = Gas1Pos.transform;
                break;
            case "Attack2":
                if (PS.sr.flipX == false)
                    go2 = Instantiate(Gas2_2, Gas2Pos.transform.position, Quaternion.identity);
                else
                    go2 = Instantiate(Gas2_1, Gas2Pos.transform.position, Quaternion.identity);
                break;
        }
    }
}
