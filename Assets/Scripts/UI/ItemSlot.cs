using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ItemSlot 클래스는 인벤토리 슬롯을 관리하고, 아이템의 정보를 표시합니다.
/// </summary>
public class ItemSlot : MonoBehaviour
{
    public ItemData item; //슬롯에 있는 아이템 데이터

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;
    public UIInventory inventory;

    public int index; // 슬롯 인덱스
    public bool equipped; // 장착 여부
    public int quantity;

    /// <summary>
    /// Awake 함수는 외곽선을 초기화합니다.
    /// </summary>
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// OnEnable 함수는 슬롯이 활성화될 때 외곽선을 업데이트합니다.
    /// </summary>
    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    /// <summary>
    /// 아이템 슬롯을 설정하고, 아이템 아이콘과 수량을 표시합니다.
    /// </summary>
    public void SetItemSlot()
    {
        icon.gameObject.SetActive(true);
        icon.sprite = item.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : string.Empty;

        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    /// <summary>
    /// 아이템 슬롯을 비우고, 아이템 아이콘과 수량을 숨깁니다.
    /// </summary>
    public void ClearItemSlot()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    /// <summary>
    /// 슬롯 버튼이 클릭되었을 때 호출되는 함수로, 인벤토리에서 해당 아이템을 선택합니다.
    /// </summary>
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
