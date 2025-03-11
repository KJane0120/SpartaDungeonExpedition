using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Condition Ŭ������ �÷��̾��� ����(ü��, ���, ���¹̳� ��)�� �����ϰ� UI�� �ݿ��մϴ�.
/// </summary>
public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;
    public Image uiBar;

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, ���� ���� ���� ������ �ʱ�ȭ�մϴ�.
    /// </summary>
    void Start()
    {
        curValue = startValue;
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ���, UI ���� ä���� ������ ������Ʈ�մϴ�.
    /// </summary>
    void Update()
    {
        uiBar.fillAmount = GetPercentage();
    }

    /// <summary>
    /// ���� ���� ������ ��ȯ�ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <returns>���� ���� ����</returns>
    float GetPercentage()
    {
        return curValue / maxValue;
    }

    /// <summary>
    /// ���� ���� �־��� ���� ���ϴ� �Լ��Դϴ�.
    /// </summary>
    /// <param name="value">���� ��</param>
    public void Add(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
    }

    /// <summary>
    /// ���� ������ �־��� ���� ���� �Լ��Դϴ�.
    /// </summary>
    /// <param name="value">�� ��</param>
    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, 0);
    }
}
