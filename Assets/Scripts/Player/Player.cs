using System;
using UnityEngine;

/// <summary>
/// Player 클래스는 플레이어 캐릭터를 정의하며, 플레이어의 컨트롤러, 상태, 장비 등을 관리합니다.
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
    /// Awake 함수는 플레이어의 컴포넌트를 초기화하고, CharacterManager에 플레이어 인스턴스를 설정합니다.
    /// </summary>
    private void Awake()
    {
        CharacterManager.Instance.Player = this;
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
        equip = GetComponent<Equipment>();
    }

}
