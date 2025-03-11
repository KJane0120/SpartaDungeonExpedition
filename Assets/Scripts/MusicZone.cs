using UnityEngine;

/// <summary>
/// MusicZone Ŭ������ �÷��̾ Ư�� ������ ���ų� ���� �� ��� ������ ������ �����մϴ�.
/// </summary>
public class MusicZone : MonoBehaviour
{
    public AudioSource source;
    public float fadeTime; //���̵� �ð�
    public float maxVolume; //�ִ� ����
    private float targetVolume; //��ǥ ����

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, ����� �ҽ��� �ʱ�ȭ�ϰ� ����� �����մϴ�.
    /// </summary>
    private void Start()
    {
        targetVolume = 0.0f;
        source = GetComponent<AudioSource>();
        source.volume = targetVolume;
        source.Play();
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ���, ����� �ҽ��� ������ ��ǥ �������� ���������� �����մϴ�.
    /// </summary>
    private void Update()
    {
        if(!Mathf.Approximately(source.volume, targetVolume))
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, (maxVolume/fadeTime)*Time.deltaTime);
        }
    }

    /// <summary>
    /// �÷��̾ Ʈ���ſ� ������ �� ȣ��Ǵ� �Լ���, ��ǥ ������ �ִ� �������� �����մϴ�.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    /// <summary>
    /// �÷��̾ Ʈ���ſ��� ������ �� ȣ��Ǵ� �Լ���, ��ǥ ������ 0���� �����մϴ�.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0.0f;
        }
    }
}
