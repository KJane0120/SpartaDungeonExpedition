using UnityEngine;

/// <summary>
/// UICondition Ŭ������ �÷��̾��� ����(ü��, ���, ���¹̳�)�� UI�� �ݿ��ϴ� ������ �մϴ�.
/// </summary>
public class UICondition : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, CharacterManager�� ���� �÷��̾��� ���¸� UICondition�� �����մϴ�.
    /// </summary>
    void Start()
    {
        CharacterManager.Instance.Player.condition.uiCondition = this;
    }
}
