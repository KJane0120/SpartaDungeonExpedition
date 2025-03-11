using UnityEngine;

/// <summary>
/// JumpPlatform 클래스는 플레이어가 충돌할 때 점프 효과를 제공하는 플랫폼을 정의합니다.
/// </summary>
public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float jumpValue;

    /// <summary>
    /// 플레이어가 플랫폼과 충돌할 때 호출되는 함수로, 플레이어에게 점프 힘을 가합니다.
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // 현재 y축 속도를 초기화하여 점프 효과를 부드럽게 만듭니다.
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                rb.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
            }
        }
    }
}
