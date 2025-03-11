using UnityEngine;

/// <summary>
/// MovePlatform Ŭ������ ���� �Ÿ���ŭ �¿�� �����̴� �÷����� �����մϴ�.
/// �÷��̾ �÷��� ���� ���� ��, �÷��̾ �÷����� �ڽ����� �����Ͽ� �Բ� �����̰� �մϴ�.
/// </summary>
public class MovePlatform : MonoBehaviour
{
    Vector3 startPos; // �÷��� ������ġ
    public float moveDistance = 10f; //�÷����� �̵��� �Ÿ�
    public float movingSpeed; //�÷��� �̵��ӵ�

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, �÷����� ���� ��ġ�� �����մϴ�.
    /// </summary>
    private void Start()
    {
        startPos = transform.position;
    }

    /// <summary>
    /// �� �����Ӹ��� ȣ��Ǵ� �Լ���, �÷����� �̵���ŵ�ϴ�.
    /// </summary>
    private void Update()
    {
        Move_Platform();
    }

    /// <summary>
    /// �÷����� �¿�� �̵���Ű�� �Լ��Դϴ�.
    /// </summary>
    void Move_Platform()
    {
        float newX = startPos.x + Mathf.PingPong(Time.time * movingSpeed, moveDistance);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// �÷��̾ �÷����� �浹�� �� ȣ��Ǵ� �Լ���, �÷��̾ �÷����� �ڽ����� �����մϴ�.
    /// </summary>
    /// <param name="collision">�浹 ����</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// �÷��̾ �÷������� ��� �� ȣ��Ǵ� �Լ���, �÷��̾ �÷����� �ڽĿ��� �����մϴ�.
    /// </summary>
    /// <param name="collision">�浹 ����</param>
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
