using UnityEngine;

/// <summary>
/// Resource Ŭ������ �ڿ��� �����ϸ�, �ڿ��� ä���ϴ� ����� �����մϴ�.
/// </summary>
public class Resource : MonoBehaviour
{
    public ItemData itemToGive; // ä�� �� ������ ������ ������
    public int quantityPerHit; // �� ���� Ÿ������ ���� �� �ִ� ������ ����
    public int capacy; // �ڿ��� �ѷ�

    /// <summary>
    /// �ڿ��� ä���ϴ� �Լ���, ������ ��ġ�� �������� �����մϴ�.
    /// </summary>
    /// <param name="hitPoint">Ÿ�� ����</param>
    /// <param name="hitNormal">Ÿ�� ������ ���� ����</param>
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
