using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ItemSlot Ŭ������ �κ��丮 ������ �����ϰ�, �������� ������ ǥ���մϴ�.
/// </summary>
public class ItemSlot : MonoBehaviour
{
    public ItemData item; //���Կ� �ִ� ������ ������

    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;
    public UIInventory inventory;

    public int index; // ���� �ε���
    public bool equipped; // ���� ����
    public int quantity;

    /// <summary>
    /// Awake �Լ��� �ܰ����� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Awake()
    {
        outline = GetComponent<Outline>();
    }

    /// <summary>
    /// OnEnable �Լ��� ������ Ȱ��ȭ�� �� �ܰ����� ������Ʈ�մϴ�.
    /// </summary>
    private void OnEnable()
    {
        outline.enabled = equipped;
    }

    /// <summary>
    /// ������ ������ �����ϰ�, ������ �����ܰ� ������ ǥ���մϴ�.
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
    /// ������ ������ ����, ������ �����ܰ� ������ ����ϴ�.
    /// </summary>
    public void ClearItemSlot()
    {
        item = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    /// <summary>
    /// ���� ��ư�� Ŭ���Ǿ��� �� ȣ��Ǵ� �Լ���, �κ��丮���� �ش� �������� �����մϴ�.
    /// </summary>
    public void OnClickButton()
    {
        inventory.SelectItem(index);
    }
}
