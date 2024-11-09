using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public Slider healthBar;           // Slider UI 컴포넌트
    public float maxHealth = 250f;      // 최대 체력
    private float currentHealth;        // 현재 체력
    private float timer = 0f;           // 시간 추적용 변수
    private bool isDead = false;        // 캐릭터의 사망 상태 확인 변수

    // Animator 컴포넌트를 연결할 변수 (사망 애니메이션 전용 오브젝트)
    public Animator deathAnimator;      // AnimationBird1 오브젝트의 Animator

    void Start()
    {
        currentHealth = maxHealth;         // 초기 체력 설정
        healthBar.maxValue = maxHealth;    // 슬라이더의 최대 값 설정
        healthBar.value = currentHealth;   // 슬라이더의 초기 값 설정
    }

    void Update()
    {
        if (isDead) return;                // 이미 사망 상태면 Update 중지

        timer += Time.deltaTime;           // 경과 시간 추적

        if (timer >= 1f)                   // 1초마다 체력을 감소
        {
            timer = 0f;
            currentHealth -= 10f;          // 체력 10 감소

            if (currentHealth <= 0)        // 체력이 0 이하로 내려가지 않도록 설정
            {
                currentHealth = 0;
                isDead = true;             // 사망 상태로 설정
                TriggerDeathAnimation();    // 사망 애니메이션 트리거
            }

            healthBar.value = currentHealth;  // 슬라이더의 값 업데이트
        }
    }

    // 사망 애니메이션 트리거 함수
    private void TriggerDeathAnimation()
    {
        if (deathAnimator != null)
        {
            deathAnimator.SetTrigger("isDead");  // 사망 애니메이션 트리거
        }
    }
}
