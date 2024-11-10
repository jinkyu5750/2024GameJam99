using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_State : MonoBehaviour
{
    public float HP = 100;
    public Animator ani;
    protected Rigidbody2D rig;
    protected CapsuleCollider2D col;
    protected SpriteRenderer sr;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected Slider HP_Slider;
    private void Start()
    {
        ani = GetComponent<Animator>();
        rig = GetComponent<Rigidbody2D>();
        col = GetComponent<CapsuleCollider2D>();
        sr = GetComponent<SpriteRenderer>();

        HP_Slider.maxValue = HP;

    }

    private void Update()
    {
        HP -= Time.deltaTime * 2f;
        HP_Slider.value = HP;
    }
}
