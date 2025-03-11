using System;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

/// <summary>
/// PlayerCondition 클래스는 플레이어의 상태를 관리하며, 체력, 허기, 스태미나를 포함합니다.
/// </summary>
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;
    public PlayerController controller;

    // UICondition에서 각 상태를 가져옵니다.
    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecrease; // 허기가 0일 때 체력 감소 값

    public event Action onTakeDamage; // 데미지를 받을 때 발생하는 이벤트

    /// <summary>
    /// 시작 시 호출되는 함수로, 플레이어 컨트롤러를 초기화합니다.
    /// </summary>
    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, 허기와 스태미나를 업데이트하고, 허기가 0일 때 체력을 감소시킵니다.
    /// </summary>
    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecrease * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// 플레이어가 죽었을 때 호출되는 함수입니다.
    /// </summary>
    public void Die()
    {
        Debug.Log("죽었다!");// 나중에 수정예정
    }

    /// <summary>
    /// 플레이어의 체력을 회복하는 함수입니다.
    /// </summary>
    /// <param name="amount">회복할 양</param>
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /// <summary>
    /// 플레이어의 허기를 회복하는 함수입니다.
    /// </summary>
    /// <param name="amount">회복할 양</param>
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    /// <summary>
    /// 플레이어에게 속도 증가 효과를 적용하는 함수입니다.
    /// </summary>
    /// <param name="value">속도 증가 값</param>
    public void ApplySpeedBoost(float value)
    {
        StartCoroutine(controller.SpeedBoostCoroutine(value, 10));
    }

    /// <summary>
    /// 플레이어가 물리적 데미지를 받을 때 호출되는 함수입니다.
    /// </summary>
    /// <param name="damage">받을 데미지 양</param>
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    /// <summary>
    /// 플레이어의 스태미나를 사용하는 함수입니다.
    /// </summary>
    /// <param name="amount">사용할 스태미나 양</param>
    /// <returns>스태미나 사용 성공 여부</returns>
    public bool UseStamina(float amount)
    {
        if(stamina.curValue - amount < 0f)
        {
            return false;
        }
        stamina.Subtract(amount);
        return true;
    }
}
