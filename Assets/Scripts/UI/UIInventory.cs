using TMPro;
using UnityEngine;

/// <summary>
/// UIInventory Ŭ������ �κ��丮 UI�� �����ϰ�, �������� �߰�, ����, ���, ���� �� ��� ����� �����մϴ�.
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
    /// ���� �� ȣ��Ǵ� �Լ���, �κ��丮�� ���õ� �ʱ� ������ �����մϴ�.
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
    /// ���õ� ������ â�� �ʱ�ȭ�մϴ�.
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
    /// �κ��丮 â�� ����մϴ�.
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
    /// �κ��丮 â�� ���� �ִ��� ���θ� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>�κ��丮 â�� ���� �ִ��� ����</returns>
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    /// <summary>
    /// �������� �κ��丮�� �߰��մϴ�.
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
    /// �κ��丮 UI�� ������Ʈ�մϴ�.
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
    /// �־��� ������ �����Ϳ� ��ġ�ϴ� ������ ������ ��ȯ�մϴ�.
    /// </summary>
    /// <param name="data">������ ������</param>
    /// <returns>��ġ�ϴ� ������ ����</returns>
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
    /// ��� �ִ� ������ ������ ��ȯ�մϴ�.
    /// </summary>
    /// <returns>��� �ִ� ������ ����</returns>
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
    /// �������� ������ ��� ��ġ�� �����մϴ�.
    /// </summary>
    /// <param name="data">������ ������</param>
    void ThrowItem(ItemData data)
    {
        Instantiate(data.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360));
    }

    /// <summary>
    /// �־��� �ε����� �������� �����մϴ�.
    /// ������Ÿ�Կ� ���� ����ϱ�/���� �� ���� ��ư�� Ȱ��ȭ�˴ϴ�.
    /// ������Ÿ�Կ� ������� ������ ��ư�� �׻� Ȱ��ȭ�˴ϴ�.
    /// </summary>
    /// <param name="index">������ ���� �ε���</param>
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
    /// ��� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ���, ���õ� �������� ����մϴ�.
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
    /// ��� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ���, ���õ� �������� ����մϴ�.
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
    /// ���õ� �������� �κ��丮���� �����մϴ�.
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
    /// ���� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ���, ���õ� �������� �����մϴ�.
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
    /// �־��� �ε����� �������� ���� �����մϴ�.
    /// </summary>
    /// <param name="index">������ ���� �ε���</param>
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
    /// ���� ���� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ���, ���õ� �������� ���� �����մϴ�.
    /// </summary>
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}
