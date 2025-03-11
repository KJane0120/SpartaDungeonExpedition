using UnityEngine;

/// <summary>
/// CharacterManager Ŭ������ �̱��� ������ ����Ͽ� ���� �� ĳ���͸� �����մϴ�.
/// </summary>
public class CharacterManager : MonoBehaviour
{
    private static CharacterManager instance; // �̱��� �ν��Ͻ�
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

    public Player player; //���� �÷��̾� ��ü
    public Player Player
    {
        get { return player; }
        set { player = value; }
    }

    /// <summary>
    /// Awake �Լ��� �̱��� �ν��Ͻ��� �ʱ�ȭ�ϰ�, �ߺ��� �ν��Ͻ��� �ı��մϴ�.
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
