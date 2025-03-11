using UnityEngine;

/// <summary>
/// EquipTool Ŭ������ �ڿ� ä�� �� ���� ����� ������ ��� �������� �����մϴ�.
/// Equip Ŭ������ ��ӹ޽��ϴ�.
/// </summary>
public class EquipTool : Equip
{
    public float attackRate;
    private bool attacking; //���� ������ ����
    public float attackDistance; //���� �Ÿ�
    public float useStamina; //�Ҹ�Ǵ� ���¹̳� ��

    [Header("Resource Gathering")]
    public bool doesGatherResources; //�ڿ� ä�� ���� ����

    [Header("Combat")]
    public bool doesDealDamage; //������ ���� �� �ִ��� ����
    public int damage;

    private Animator animator;
    private Camera _camera;

    /// <summary>
    /// �ִϸ����Ϳ� ī�޶� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Start()
    {
        animator = GetComponent<Animator>();
        _camera = Camera.main;
    }

    /// <summary>
    /// ���� �Է��� ������ �� ȣ��Ǵ� �Լ���, ���¹̳��� �Ҹ��ϰ� ������ �����մϴ�.
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
    /// ���� ���� ���·� ��ȯ�ϴ� �Լ��Դϴ�.
    /// �ణ�� �ð����� �α� ����, ���� �Լ��� �����߽��ϴ�. 
    /// </summary>
    void OnCanAttack()
    {
        attacking = false;
    }

    /// <summary>
    /// ���� �� ȣ��Ǵ� �Լ���, �ڿ� ä�� �� �������� ���մϴ�.
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
