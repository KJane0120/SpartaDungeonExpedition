using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Equipment Ŭ������ �÷��̾��� ��� �����ϰ�, ����� ���� �Է��� ó���մϴ�.
/// </summary>
public class Equipment : MonoBehaviour
{
    public Equip curEquip; //���� ������ ���
    public Transform equipParent; //��� ������ �θ� Ʈ������

    private PlayerController controller;
    private PlayerCondition condition;

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, �÷��̾� ��Ʈ�ѷ��� ���¸� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    /// <summary>
    /// ���ο� ��� �����ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="data">������ ������ ������</param>
    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    /// <summary>
    /// ���� ������ ��� �����ϴ� �Լ��Դϴ�.
    /// </summary>
    public void UnEquip()
    {
        if (curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    /// <summary>
    /// ���� �Է��� ������ �� ȣ��Ǵ� �Լ���, ���� ������ ����� ���� �Է��� ó���մϴ�.
    /// </summary>
    /// <param name="context">�Է� ���ؽ�Ʈ</param>
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}
