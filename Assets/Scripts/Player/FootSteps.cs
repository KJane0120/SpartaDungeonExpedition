using UnityEngine;

/// <summary>
/// FootSteps Ŭ������ �÷��̾��� �߼Ҹ��� ����ϴ� ����� �����մϴ�.
/// </summary>
public class FootSteps : MonoBehaviour
{
    public AudioClip[] footstepClips;
    private AudioSource audioSource;
    private Rigidbody rb;
    public float footstepThreshold;
    public float footstepRate;
    private float footStepTime;

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, ������ٵ�� ����� �ҽ��� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ���, �÷��̾��� �ӵ��� Ȯ���ϰ� �߼Ҹ��� ����մϴ�.
    /// </summary>
    private void Update()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.1f) //�÷��̾ ���� ���� �ƴ� �� üũ
        {
            if(rb.velocity.magnitude > footstepThreshold) // �÷��̾� �ӵ��� �Ӱ谪�� �ʰ��� �� üũ
            {
                if(Time.time -footStepTime > footstepRate) //�߼Ҹ� ��� ������ ������ �� üũ
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]); //������ �߼Ҹ� Ŭ�� ���
                }
            }
        }
    }
}
