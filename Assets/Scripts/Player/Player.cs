using System;
using UnityEngine;

/// <summary>
/// Player Ŭ������ �÷��̾� ĳ���͸� �����ϸ�, �÷��̾��� ��Ʈ�ѷ�, ����, ��� ���� �����մϴ�.
/// </summary>
public class Player : MonoBehaviour
{
    public PlayerController controller;
    public PlayerCondition condition;
    public Equipment equip;

    public ItemData itemData;
    public Action addItem;

    public Transform dropItemPosition;

    /// <summary>
    /// Awake �Լ��� �÷��̾��� ������Ʈ�� �ʱ�ȭ�ϰ�, CharacterManager�� �÷��̾� �ν��Ͻ��� �����մϴ�.
    /// </summary>
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }

}
