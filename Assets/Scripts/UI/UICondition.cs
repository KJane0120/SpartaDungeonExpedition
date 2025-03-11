using UnityEngine;

/// <summary>
/// UICondition 클래스는 플레이어의 상태(체력, 허기, 스태미나)를 UI에 반영하는 역할을 합니다.
/// </summary>
public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    /// <summary>
    /// 시작 시 호출되는 함수로, CharacterManager를 통해 플레이어의 상태를 UICondition에 연결합니다.
    /// </summary>
    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
