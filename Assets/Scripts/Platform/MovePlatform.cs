using UnityEngine;

/// <summary>
/// MovePlatform 클래스는 일정 거리만큼 좌우로 움직이는 플랫폼을 정의합니다.
/// 플레이어가 플랫폼 위에 있을 때, 플레이어를 플랫폼의 자식으로 설정하여 함께 움직이게 합니다.
/// </summary>
public class MovePlatform : MonoBehaviour
{
    Vector3 startPos; // 플랫폼 시작위치
    public float moveDistance = 10f; //플랫폼이 이동할 거리
    public float movingSpeed; //플랫폼 이동속도

    /// <summary>
    /// 시작 시 호출되는 함수로, 플랫폼의 시작 위치를 저장합니다.
    /// </summary>
    private void Start()
    {
        startPos = transform.position;
    }

    /// <summary>
    /// 매 프레임마다 호출되는 함수로, 플랫폼을 이동시킵니다.
    /// </summary>
    private void Update()
    {
        Move_Platform();
    }

    /// <summary>
    /// 플랫폼을 좌우로 이동시키는 함수입니다.
    /// </summary>
    void Move_Platform()
    {
        float newX = startPos.x + Mathf.PingPong(Time.time * movingSpeed, moveDistance);
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
    }

    /// <summary>
    /// 플레이어가 플랫폼과 충돌할 때 호출되는 함수로, 플레이어를 플랫폼의 자식으로 설정합니다.
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player") && collision.transform.position.y > transform.position.y)
        {
            collision.transform.SetParent(transform);
        }
    }

    /// <summary>
    /// 플레이어가 플랫폼에서 벗어날 때 호출되는 함수로, 플레이어를 플랫폼의 자식에서 해제합니다.
    /// </summary>
    /// <param name="collision">충돌 정보</param>
    private void OnCollisionExit(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }
}
