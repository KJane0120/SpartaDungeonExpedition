using UnityEngine;

/// <summary>
/// EquipTool 클래스는 자원 채집 및 전투 기능을 포함한 장비 아이템을 정의합니다.
/// Equip 클래스를 상속받습니다.
/// </summary>
public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking; //공격 중인지 여부
    public float attackDistance; //공격 거리
    public float useStamina; //소모되는 스태미나 양

    [Header("Resource Gathering")]
    public bool doesGatherResources; //자원 채집 가능 여부

    [Header("Combat")]
    public bool doesDealDamage; //데미지 가할 수 있는지 여부
    public int damage;

    private Animator animator;
    private Camera _camera;

    /// <summary>
    /// 애니메이터와 카메라를 초기화합니다.
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    /// <summary>
    /// 공격 입력이 들어왔을 때 호출되는 함수로, 스태미나를 소모하고 공격을 시작합니다.
    /// </summary>
    public override void OnAttackInput()
    {
        if (!attacking)
        {
            if (CharacterManager.Instance.Player.condition.UseStamina(useStamina))
            {
                attacking = true;
                animator.SetTrigger("Attack");
                Invoke("OnCanAttack", attackRate);
            }
        }
    }

    /// <summary>
    /// 공격 가능 상태로 전환하는 함수입니다.
    /// 약간의 시간차를 두기 위해, 따로 함수를 생성했습니다. 
    /// </summary>
    void OnCanAttack()
    {
        attacking = false;
    }

    /// <summary>
    /// 공격 시 호출되는 함수로, 자원 채집 및 데미지를 가합니다.
    /// </summary>
    public void OnHit()
    {
        Ray ray = _camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, attackDistance))
        {
            if (doesGatherResources && hit.collider.TryGetComponent(out Resource resource))
            {
                resource.Gather(hit.point, hit.normal);
            }
            if (doesDealDamage && hit.collider.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakePhysicalDamage(damage);
            }
        }
    }
}
