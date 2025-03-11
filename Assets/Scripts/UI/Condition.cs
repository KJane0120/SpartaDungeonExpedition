using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Condition 클래스는 플레이어의 상태(체력, 허기, 스태미나 등)를 관리하고 UI에 반영합니다.
/// </summary>
public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    /// <summary>
    /// 시작 시 호출되는 함수로, 현재 값을 시작 값으로 초기화합니다.
    /// </summary>
    void Start()
    {
        curValue = startValue;
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, UI 바의 채워진 정도를 업데이트합니다.
    /// </summary>
    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    /// <summary>
    /// 현재 값의 비율을 반환하는 함수입니다.
    /// </summary>
    /// <returns>현재 값의 비율</returns>
    float GetPercentage()
    {
        return curValue / maxValue;
    }

    /// <summary>
    /// 현재 값에 주어진 값을 더하는 함수입니다.
    /// </summary>
    /// <param name="value">더할 값</param>
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    /// <summary>
    /// 현재 값에서 주어진 값을 빼는 함수입니다.
    /// </summary>
    /// <param name="value">뺄 값</param>
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
