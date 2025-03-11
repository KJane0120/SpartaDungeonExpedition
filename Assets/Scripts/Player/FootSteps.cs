using UnityEngine;

/// <summary>
/// FootSteps 클래스는 플레이어의 발소리를 재생하는 기능을 제공합니다.
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
    /// 시작 시 호출되는 함수로, 리지드바디와 오디오 소스를 초기화합니다.
    /// </summary>
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, 플레이어의 속도를 확인하고 발소리를 재생합니다.
    /// </summary>
    private void Update()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.1f) //플레이어가 점프 중이 아닐 때 체크
        {
            if(rb.velocity.magnitude > footstepThreshold) // 플레이어 속도가 임계값을 초과할 때 체크
            {
                if(Time.time -footStepTime > footstepRate) //발소리 재생 간격이 지났을 때 체크
                {
                    footStepTime = Time.time;
                    audioSource.PlayOneShot(footstepClips[Random.Range(0, footstepClips.Length)]); //랜덤한 발소리 클립 재생
                }
            }
        }
    }
}
