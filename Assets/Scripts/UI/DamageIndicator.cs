using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DamageIndicator Ŭ������ �÷��̾ �������� ���� �� ȭ�鿡 �÷��� ȿ���� ǥ���մϴ�.
/// </summary>
public class DamageIndicator : MonoBehaviour
{
    public Image image; //������ �÷��� ǥ�� �̹���
    public float flashSpeed; //�÷��ð� ������� �ӵ�

    private Coroutine coroutine; 

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, �÷��̾ �������� ���� �� Flash �Լ��� ȣ���ϵ��� �̺�Ʈ�� ����մϴ�.
    /// </summary>
    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    /// <summary>
    /// ������ �÷��ø� �����ϴ� �Լ��Դϴ�.
    /// </summary>
    public void Flash()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        image.enabled = true;
        image.color = new Color(1f, 100f / 255f, 100f / 255f);
        coroutine = StartCoroutine(FadeAway());
    }

    /// <summary>
    /// ������ �÷��ø� ������ ������� �ϴ� �ڷ�ƾ�Դϴ�.
    /// </summary>
    /// <returns>�ڷ�ƾ</returns>
    private IEnumerator FadeAway()
    {
        float startAlpha = 0.3f;
        float a = startAlpha;

        while(a > 0)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(1f, 100f / 255f, 100f / 255f, a);
            yield return null;
        }
        image.enabled = false;
    }
}
