using UnityEngine;

/// <summary>
/// IInteractable 인터페이스는 상호작용 가능한 객체의 기본 동작을 정의합니다.
/// </summary>
public interface IInteractable
{
    /// <summary>
    /// 상호작용 프롬프트를 반환합니다.
    /// </summary>
    /// <returns>상호작용 프롬프트 문자열</returns>
    public string GetInteractPrompt();

    /// <summary>
    /// 상호작용 시 호출되는 함수입니다.
    /// </summary>
    public void OnInteract();
}

/// <summary>
/// ItemObject 클래스는 게임 내 아이템 객체를 정의하며, 상호작용 기능을 제공합니다.
/// </summary>
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData data;

    /// <summary>
    /// 아이템의 이름과 설명을 포함한 상호작용 프롬프트를 반환합니다.
    /// </summary>
    /// <returns>아이템의 이름과 설명을 포함한 문자열</returns>
    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    /// <summary>
    /// 아이템과 상호작용 시 호출되는 함수로, 아이템을 플레이어가 가진 아이템 데이터에 추가하고 객체를 파괴합니다.
    /// </summary>
    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = data;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject);
    }
}
