using System;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource,
}
/// <summary>
/// 현 단계에서 JumpCount, Invincibility에 해당하는 타입의 아이템은 없습니다. 
/// </summary>
public enum ConsumableType
{
    Health,
    Hunger,
    SpeedBoost,
    JumpCount,
    Invincibility
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;
}

[CreateAssetMenu(fileName ="Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;

}
