using UnityEngine;

/// <summary>
/// DayNightCycle 클래스는 게임 내에서 낮과 밤의 주기를 관리합니다.
/// 태양과 달의 조명 색상, 강도 및 기타 조명 설정을 시간에 따라 업데이트합니다.
/// </summary>
public class DayNightCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time; // 현재 시간 (0.0f ~ 1.0f)
    public float fullDayLength; // 하루의 길이 (초 단위)
    public float startTime = 0.4f; // 시작 시간 (0.0f ~ 1.0f)
    private float timeRate;
    public Vector3 noon; // 정오의 태양 위치, Vector3(90,0,0)

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Other Lighting")]
    public AnimationCurve lightingIntensityMultiplier; // 주변 조명 강도 변화
    public AnimationCurve reflectionIntensityMultiplier; // 반사 조명 강도 변화


    /// <summary>
    /// 시작 시 호출되는 함수로, 시간 증가율을 계산하고 시작 시간을 설정합니다.
    /// </summary>
    void Start()
    {
        timeRate = 1.0f /fullDayLength;
        time = startTime;
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, 현재 시간을 업데이트하고 조명을 갱신합니다.
    /// </summary>
    void Update()
    {
        time = (time + timeRate * Time.deltaTime) % 1.0f;

        UpdateLighting(sun, sunColor, sunIntensity);
        UpdateLighting(moon, moonColor, moonIntensity);

        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionIntensityMultiplier.Evaluate(time);
    }

    /// <summary>
    /// 주어진 조명 소스의 색상과 강도를 업데이트합니다.
    /// </summary>
    /// <param name="lightSource">조명 소스</param>
    /// <param name="gradient">색상 변화</param>
    /// <param name="intensityCurve">강도 변화</param>
    void UpdateLighting(Light lightSource, Gradient gradient, AnimationCurve intensityCurve)
    {
        float intensity = intensityCurve.Evaluate(time);

        lightSource.transform.eulerAngles = (time - (lightSource == sun ? 0.25f : 0.75f))*noon*4f;
        lightSource.color = gradient.Evaluate(time);
        lightSource.intensity = intensity;

        GameObject go = lightSource.gameObject;
        if(lightSource.intensity == 0 && go.activeInHierarchy)
        {
            go.SetActive(false);
        }
        else if(lightSource.intensity >0 && !go.activeInHierarchy)
        {
            go.SetActive(true);
        }
    }
}
