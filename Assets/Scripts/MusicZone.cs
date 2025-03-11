using UnityEngine;

/// <summary>
/// MusicZone 클래스는 플레이어가 특정 구역에 들어가거나 나갈 때 배경 음악의 볼륨을 조절합니다.
/// </summary>
public class MusicZone : MonoBehaviour
{
    public AudioSource source;
    public float fadeTime; //페이드 시간
    public float maxVolume; //최대 볼륨
    private float targetVolume; //목표 볼륨

    /// <summary>
    /// 시작 시 호출되는 함수로, 오디오 소스를 초기화하고 재생을 시작합니다.
    /// </summary>
    private void Start()
    {
        targetVolume = 0.0f;
        source = GetComponent<AudioSource>();
        source.volume = targetVolume;
        source.Play();
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, 오디오 소스의 볼륨을 목표 볼륨으로 점진적으로 변경합니다.
    /// </summary>
    private void Update()
    {
        if(!Mathf.Approximately(source.volume, targetVolume))
        {
            source.volume = Mathf.MoveTowards(source.volume, targetVolume, (maxVolume/fadeTime)*Time.deltaTime);
        }
    }

    /// <summary>
    /// 플레이어가 트리거에 들어왔을 때 호출되는 함수로, 목표 볼륨을 최대 볼륨으로 설정합니다.
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = maxVolume;
        }
    }

    /// <summary>
    /// 플레이어가 트리거에서 나갔을 때 호출되는 함수로, 목표 볼륨을 0으로 설정합니다.
    /// </summary>
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetVolume = 0.0f;
        }
    }
}
