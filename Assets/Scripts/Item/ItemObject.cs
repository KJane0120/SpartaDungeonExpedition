using UnityEngine;

/// <summary>
/// IInteractable �������̽��� ��ȣ�ۿ� ������ ��ü�� �⺻ ������ �����մϴ�.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// ��ȣ�ۿ� ������Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>��ȣ�ۿ� ������Ʈ ���ڿ�</returns>
    public string GetInteractPrompt();

    /// <summary>
    /// ��ȣ�ۿ� �� ȣ��Ǵ� �Լ��Դϴ�.
    /// </summary>
    public void OnInteract();
}

/// <summary>
/// ItemObject Ŭ������ ���� �� ������ ��ü�� �����ϸ�, ��ȣ�ۿ� ����� �����մϴ�.
/// </summary>
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    /// <summary>
    /// �������� �̸��� ������ ������ ��ȣ�ۿ� ������Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>�������� �̸��� ������ ������ ���ڿ�</returns>
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    /// <summary>
    /// �����۰� ��ȣ�ۿ� �� ȣ��Ǵ� �Լ���, �������� �÷��̾ ���� ������ �����Ϳ� �߰��ϰ� ��ü�� �ı��մϴ�.
    /// </summary>
    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
