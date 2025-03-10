using System;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;

    public PlayerController controller;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecrease;

    private void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
    }
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

    public void Die()
    {
        Debug.Log("�׾���!");// ���߿� ����
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void ApplySpeedBoost(float value)
    {
        StartCoroutine(controller.SpeedBoostCoroutine(value, 10));
        //Invincibility�� ��� ���� �߰� ����->������ ���� ���� ��, ������ �޴� �κп��� ȣ��
    }
}
