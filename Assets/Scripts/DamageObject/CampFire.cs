using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CampFire 클래스는 주기적으로 데미지를 가하는 기능을 합니다.
/// 트리거에 들어오고 나가는 객체들을 관리하며, 일정 시간마다 데미지를 가합니다.
/// </summary>
public class CampFire : MonoBehaviour
{
    public int damage;
    public float damageRate;

    // 데미지를 받을 수 있는 객체들의 리스트
    List<IDamageable> things = new List<IDamageable>();
    void Start()
    {
        // 일정 시간마다 DealDamage 함수를 호출
        InvokeRepeating("DealDamage", 0, damageRate);
    }

    // 리스트에 있는 모든 객체에 데미지를 가하는 함수
    void DealDamage()
    {
        for(int i = 0; i < things.Count; i++)
        {
            things[i].TakePhysicalDamage(damage);
        }
    }

    // 트리거에 다른 객체가 들어왔을 때 호출되는 함수
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            things.Add(damageable);
        }
    }

    // 트리거에서 다른 객체가 나갔을 때 호출되는 함수
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            things.Remove(damageable);
        }
    }
}
