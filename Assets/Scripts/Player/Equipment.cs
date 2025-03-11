using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Equipment 클래스는 플레이어의 장비를 관리하고, 장비의 공격 입력을 처리합니다.
/// </summary>
public class Equipment : MonoBehaviour
{
    public Equip curEquip; //현재 장착된 장비
    public Transform equipParent; //장비가 부착될 부모 트랜스폼

    private PlayerController controller;
    private PlayerCondition condition;

    /// <summary>
    /// 시작 시 호출되는 함수로, 플레이어 컨트롤러와 상태를 초기화합니다.
    /// </summary>
    private void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    /// <summary>
    /// 새로운 장비를 장착하는 함수입니다.
    /// </summary>
    /// <param name="data">장착할 아이템 데이터</param>
    public void EquipNew(ItemData data)
    {
        UnEquip();
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    /// <summary>
    /// 현재 장착된 장비를 해제하는 함수입니다.
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
    /// 공격 입력이 들어왔을 때 호출되는 함수로, 현재 장착된 장비의 공격 입력을 처리합니다.
    /// </summary>
    /// <param name="context">입력 컨텍스트</param>
    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}
