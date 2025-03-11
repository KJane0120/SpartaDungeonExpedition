using UnityEngine;

/// <summary>
/// CharacterManager 클래스는 싱글톤 패턴을 사용하여 게임 내 캐릭터를 관리합니다.
/// </summary>
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance; // 싱글톤 인스턴스
    public static CharacterManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            }
            return instance;
        }
    }

    public Player player; //현재 플레이어 객체
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    /// <summary>
    /// Awake 함수는 싱글톤 인스턴스를 초기화하고, 중복된 인스턴스를 파괴합니다.
    /// </summary>
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}
