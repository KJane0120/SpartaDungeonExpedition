using UnityEngine;

/// <summary>
/// Equip 클래스는 장비 아이템의 기본 동작을 정의합니다.
/// </summary>
public class Equip : MonoBehaviour
{
    /// <summary>
    /// 공격 입력이 들어왔을 때 호출되는 가상 함수입니다.
    /// 기본적으로는 아무 동작도 하지 않으며, 자식 클래스에서 재정의하여 사용합니다.
    /// </summary>
    public virtual void OnAttackInput()
    {

    }
}
