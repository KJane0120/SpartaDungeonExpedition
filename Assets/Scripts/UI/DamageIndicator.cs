using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DamageIndicator 클래스는 플레이어가 데미지를 받을 때 화면에 플래시 효과를 표시합니다.
/// </summary>
public class DamageIndicator : MonoBehaviour
{
    public Image image; //데미지 플래시 표시 이미지
    public float flashSpeed; //플래시가 사라지는 속도

    private Coroutine coroutine; 

    /// <summary>
    /// 시작 시 호출되는 함수로, 플레이어가 데미지를 받을 때 Flash 함수를 호출하도록 이벤트를 등록합니다.
    /// </summary>
    void Start()
    {
        CharacterManager.Instance.Player.condition.onTakeDamage += Flash;
    }

    /// <summary>
    /// 데미지 플래시를 시작하는 함수입니다.
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
    /// 데미지 플래시를 서서히 사라지게 하는 코루틴입니다.
    /// </summary>
    /// <returns>코루틴</returns>
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
