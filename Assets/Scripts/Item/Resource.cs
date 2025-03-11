using UnityEngine;

/// <summary>
/// Resource 클래스는 자원을 정의하며, 자원을 채집하는 기능을 제공합니다.
/// </summary>
public class Resource : MonoBehaviour
{
    public ItemData itemToGive; // 채집 시 제공할 아이템 데이터
    public int quantityPerHit; // 한 번의 타격으로 얻을 수 있는 아이템 수량
    public int capacy; // 자원의 총량

    /// <summary>
    /// 자원을 채집하는 함수로, 지정된 위치에 아이템을 생성합니다.
    /// </summary>
    /// <param name="hitPoint">타격 지점</param>
    /// <param name="hitNormal">타격 지점의 법선 벡터</param>
    public void Gather(Vector3 hitPoint, Vector3 hitNormal)
    {
        for(int i = 0; i< quantityPerHit; i++)
        {
            if (capacy <= 0) break;
            capacy -= 1;
            Instantiate(itemToGive.dropPrefab, hitPoint + Vector3.up, Quaternion.LookRotation(hitNormal, Vector3.up));
        }
    }
}
