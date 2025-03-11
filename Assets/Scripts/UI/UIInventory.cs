using TMPro;
using UnityEngine;

/// <summary>
/// UIInventory 클래스는 인벤토리 UI를 관리하고, 아이템의 추가, 선택, 사용, 장착 및 드롭 기능을 제공합니다.
/// </summary>
public class UIInventory : MonoBehaviour
{
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform slotPanel;
    public Transform dropPosition;

    [Header("Select Item")]
    private ItemData selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedStatName;
    public TextMeshProUGUI selectedStatValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private PlayerController controller;
    private PlayerCondition condition;

    private int curEquipIndex;

    /// <summary>
    /// 시작 시 호출되는 함수로, 인벤토리와 관련된 초기 설정을 수행합니다.
    /// </summary>
    void Start()
    {
        controller = CharacterManager.Instance.Player.controller;
        condition = CharacterManager.Instance.Player.condition;
        dropPosition = CharacterManager.Instance.Player.dropItemPosition;

        controller.inventory += Toggle;
        CharacterManager.Instance.Player.addItem += AddItem;

        inventoryWindow.SetActive(false);
        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].ClearItemSlot();
        }

        ClearSelectedItemWindow();
    }

    /// <summary>
    /// 선택된 아이템 창을 초기화합니다.
    /// </summary>
    void ClearSelectedItemWindow()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    /// <summary>
    /// 인벤토리 창을 토글합니다.
    /// </summary>
    public void Toggle()
    {
        if (IsOpen())
        {
            inventoryWindow.SetActive(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
        }
    }

    /// <summary>
    /// 인벤토리 창이 열려 있는지 여부를 반환합니다.
    /// </summary>
    /// <returns>인벤토리 창이 열려 있는지 여부</returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    /// <summary>
    /// 아이템을 인벤토리에 추가합니다.
    /// </summary>
    public void AddItem()
    {
        ItemData data = CharacterManager.Instance.Player.itemData;

        if (data.canStack)
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                CharacterManager.Instance.Player.itemData = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = data;
            emptySlot.quantity = 1;
            UpdateUI();
            CharacterManager.Instance.Player.itemData = null;
            return;
        }

        ThrowItem(data);
        CharacterManager.Instance.Player.itemData = null;
    }

    /// <summary>
    /// 인벤토리 UI를 업데이트합니다.
    /// </summary>
    public void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                slots[i].SetItemSlot();
            }
            else
            {
                slots[i].ClearItemSlot();
            }
        }
    }

    /// <summary>
    /// 주어진 아이템 데이터와 일치하는 아이템 슬롯을 반환합니다.
    /// </summary>
    /// <param name="data">아이템 데이터</param>
    /// <returns>일치하는 아이템 슬롯</returns>
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == data && slots[i].quantity < data.maxStackAmount)
            {
                return slots[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 비어 있는 아이템 슬롯을 반환합니다.
    /// </summary>
    /// <returns>비어 있는 아이템 슬롯</returns>
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                return slots[i];
            }
        }
        return null;
    }

    /// <summary>
    /// 아이템을 랜덤한 드롭 위치에 생성합니다.
    /// </summary>
    /// <param name="data">아이템 데이터</param>
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    /// <summary>
    /// 주어진 인덱스의 아이템을 선택합니다.
    /// 아이템타입에 따라 사용하기/장착 및 해제 버튼이 활성화됩니다.
    /// 아이템타입에 관계없이 버리기 버튼은 항상 활성화됩니다.
    /// </summary>
    /// <param name="index">아이템 슬롯 인덱스</param>
    public void SelectItem(int index)
    {
        if (slots[index].item == null) return;

        selectedItem = slots[index].item;
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.displayName;
        selectedItemDescription.text = selectedItem.description;
        selectedStatName.text = string.Empty;
        selectedStatValue.text = string.Empty;

        for (int i = 0; i < selectedItem.consumables.Length; i++)
        {
            selectedStatName.text += selectedItem.consumables[i].type.ToString() + "\n";
            selectedStatValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        dropButton.SetActive(true);
    }

    /// <summary>
    /// 사용 버튼이 클릭되었을 때 호출되는 함수로, 선택된 아이템을 사용합니다.
    /// </summary>
    public void OnUseButton()
    {
        if (selectedItem.type == ItemType.Consumable)
        {
            for (int i = 0; i < selectedItem.consumables.Length; i++)
            {
                switch (selectedItem.consumables[i].type)
                {
                    case ConsumableType.Health:
                        condition.Heal(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.Hunger:
                        condition.Eat(selectedItem.consumables[i].value);
                        break;
                    case ConsumableType.SpeedBoost:
                        condition.ApplySpeedBoost(selectedItem.consumables[i].value);
                        break;
                }
            }
            RemoveSelectedItem();
        }
    }

    /// <summary>
    /// 드롭 버튼이 클릭되었을 때 호출되는 함수로, 선택된 아이템을 드롭합니다.
    /// </summary>
    public void OnDropButton()
    {
        if(CharacterManager.Instance.Player.equip.curEquip != null)
        {
            UnEquip(curEquipIndex);
        }
        ThrowItem(selectedItem);
        RemoveSelectedItem();
    }

    /// <summary>
    /// 선택된 아이템을 인벤토리에서 제거합니다.
    /// </summary>
    void RemoveSelectedItem()
    {
        slots[selectedItemIndex].quantity--;

        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].item = null;
            selectedItemIndex = -1;
            ClearSelectedItemWindow();
        }
        UpdateUI();
    }

    /// <summary>
    /// 장착 버튼이 클릭되었을 때 호출되는 함수로, 선택된 아이템을 장착합니다.
    /// </summary>
    public void OnEquipButtion()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        CharacterManager.Instance.Player.equip.EquipNew(selectedItem);
        UpdateUI();

        SelectItem(selectedItemIndex);
    }

    /// <summary>
    /// 주어진 인덱스의 아이템을 장착 해제합니다.
    /// </summary>
    /// <param name="index">아이템 슬롯 인덱스</param>
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        CharacterManager.Instance.Player.equip.UnEquip();
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }

    /// <summary>
    /// 장착 해제 버튼이 클릭되었을 때 호출되는 함수로, 선택된 아이템을 장착 해제합니다.
    /// </summary>
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
