using System;
using UnityEngine;

public interface IDamageable
{
    void TakePhysicalDamage(int damage);
}

/// <summary>
/// PlayerCondition Ŭ������ �÷��̾��� ���¸� �����ϸ�, ü��, ���, ���¹̳��� �����մϴ�.
/// </summary>
public class PlayerCondition : MonoBehaviour, IDamageable
{
    public UICondition uiCondition;
    public PlayerController controller;

    // UICondition���� �� ���¸� �����ɴϴ�.
    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecrease; // ��Ⱑ 0�� �� ü�� ���� ��

    public event Action onTakeDamage; // �������� ���� �� �߻��ϴ� �̺�Ʈ

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, �÷��̾� ��Ʈ�ѷ��� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ���, ���� ���¹̳��� ������Ʈ�ϰ�, ��Ⱑ 0�� �� ü���� ���ҽ�ŵ�ϴ�.
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
    /// �÷��̾ �׾��� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// </summary>
    public void Die()
    {
        Debug.Log("�׾���!");// ���߿� ��������
    }

    /// <summary>
    /// �÷��̾��� ü���� ȸ���ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="amount">ȸ���� ��</param>
    public void Heal(float amount)
    {
        health.Add(amount);
    }

    /// <summary>
    /// �÷��̾��� ��⸦ ȸ���ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="amount">ȸ���� ��</param>
    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    /// <summary>
    /// �÷��̾�� �ӵ� ���� ȿ���� �����ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="value">�ӵ� ���� ��</param>
    public void ApplySpeedBoost(float value)
    {
        StartCoroutine(controller.SpeedBoostCoroutine(value, 10));
    }

    /// <summary>
    /// �÷��̾ ������ �������� ���� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }

    /// <summary>
    /// �÷��̾��� ���¹̳��� ����ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="amount">����� ���¹̳� ��</param>
    /// <returns>���¹̳� ��� ���� ����</returns>
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
