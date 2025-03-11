using UnityEngine;

/// <summary>
/// JumpPlatform Ŭ������ �÷��̾ �浹�� �� ���� ȿ���� �����ϴ� �÷����� �����մϴ�.
/// </summary>
public class JumpPlatform : MonoBehaviour
{
    [SerializeField] private float jumpValue;

    /// <summary>
    /// �÷��̾ �÷����� �浹�� �� ȣ��Ǵ� �Լ���, �÷��̾�� ���� ���� ���մϴ�.
    /// </summary>
    /// <param name="collision">�浹 ����</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();

            if (rb != null)
            {
                // ���� y�� �ӵ��� �ʱ�ȭ�Ͽ� ���� ȿ���� �ε巴�� ����ϴ�.
                rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

                rb.AddForce(Vector3.up * jumpValue, ForceMode.Impulse);
            }
        }
    }
}
