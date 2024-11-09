using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_State : MonoBehaviour
{

    protected Animator ani;
    protected Rigidbody2D rig;
    protected CapsuleCollider2D col;
    protected SpriteRenderer sr;
    [SerializeField] protected float moveSpeed;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();
    }
}
