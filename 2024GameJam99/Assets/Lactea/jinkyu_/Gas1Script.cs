using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gas1Script : MonoBehaviour
{
    BoxCollider2D col;


    private void Start()
    {
        Destroy(gameObject, 1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Chicken"))
        {
            if (transform.name == "가스_1(Clone)")
                collision.gameObject.GetComponent<ChickenObject>().TakeDamage(10, 25);
            else if (transform.name == "가스_2(Clone)")
                collision.gameObject.GetComponent<ChickenObject>().TakeDamage(0, 30);
        }
    }


   
}
