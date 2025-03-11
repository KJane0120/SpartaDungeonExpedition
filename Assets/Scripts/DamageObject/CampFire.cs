using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CampFire Ŭ������ �ֱ������� �������� ���ϴ� ����� �մϴ�.
/// Ʈ���ſ� ������ ������ ��ü���� �����ϸ�, ���� �ð����� �������� ���մϴ�.
/// </summary>
public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    // �������� ���� �� �ִ� ��ü���� ����Ʈ
    List<IDamageable> things = new List<IDamageable>();
    void Start()
    {
        // ���� �ð����� DealDamage �Լ��� ȣ��
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    // ����Ʈ�� �ִ� ��� ��ü�� �������� ���ϴ� �Լ�
    void DealDamage()
    {
        for(int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // Ʈ���ſ� �ٸ� ��ü�� ������ �� ȣ��Ǵ� �Լ�
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    // Ʈ���ſ��� �ٸ� ��ü�� ������ �� ȣ��Ǵ� �Լ�
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}
