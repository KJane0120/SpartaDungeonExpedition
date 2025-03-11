using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NPC�� AI ���¸� �����ϴ� ������
/// </summary>
public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

/// <summary>
/// NPC Ŭ������ AI ���¿� ���� �ൿ�ϴ� NPC�� �����մϴ�.
/// </summary>
public class NPC : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath; //�׾��� �� ��� ������

    [Header("AI")]
    private NavMeshAgent agent;
    public float dectectDistance; //�÷��̾ �����ϴ� �Ÿ�
    private AIState aiState; //���� AI ����

    [Header("Wandering")]
    public float minWanderingDistance; //�ּ� ��Ȳ �Ÿ�
    public float maxWanderingDistance; //�ִ� ��Ȳ �Ÿ�
    public float minWanderWaitTime; //�ּ� ��Ȳ ���ð�
    public float maxWanderWaitTime; //�ִ� ��Ȳ ���ð�
    public float safeDistance; //���� �Ÿ�

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime; //������ ���� �ð�
    public float attackDistance;

    private float playerDistance; //�÷��̾���� �Ÿ�

    public float fieldOfView = 120f; //�þ߰�

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    /// <summary>
    /// ������Ʈ�� �ʱ�ȭ�մϴ�.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// ���� �� AI ���¸� Wandering���� �����մϴ�.
    /// </summary>
    private void Start()
    {
        SetState(AIState.Wandering);
    }

    /// <summary>
    /// �� �����Ӹ��� AI ���¿� ���� �ൿ�� ������Ʈ�մϴ�.
    /// </summary>
    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, CharacterManager.Instance.Player.transform.position);

        animator.SetBool("Moving", aiState != AIState.Idle);

        switch (aiState)
        {
            case AIState.Idle:
            case AIState.Wandering:
                PassiveUpdate();
                break;
            case AIState.Attacking:
                AttackingUpdate();
                break;
            case AIState.Fleeing:
                FleeingUpdate();
                break;
        }
    }

    /// <summary>
    /// AI ���¸� �����մϴ�.
    /// </summary>
    /// <param name="state">������ AI ����</param>
    public void SetState(AIState state)
    {
        aiState = state;

        switch (aiState)
        {
            case AIState.Idle:
                agent.speed = walkSpeed;
                agent.isStopped = true;
                break;
            case AIState.Wandering:
                agent.speed = walkSpeed;
                agent.isStopped = false;
                break;
            case AIState.Attacking:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
            case AIState.Fleeing:
                agent.speed = runSpeed;
                agent.isStopped = false;
                break;
        }

        animator.speed = agent.speed / walkSpeed;
    }

    /// <summary>
    /// Idle �Ǵ� Wandering ���¿����� �ൿ�� ������Ʈ�մϴ�.
    /// </summary>
    void PassiveUpdate()
    {
        if (aiState == AIState.Wandering && agent.remainingDistance < 0.1f)
        {
            SetState(AIState.Idle);
            Invoke("WanderToNewLocation", Random.Range(minWanderWaitTime, maxWanderWaitTime));
        }

        if (playerDistance < dectectDistance)
        {
            SetState(AIState.Attacking);
        }
    }

    /// <summary>
    /// ���ο� ��ġ�� ��Ȳ�մϴ�.
    /// </summary>
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    /// <summary>
    /// ��Ȳ�� ��ġ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>��Ȳ�� ��ġ</returns>
    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderingDistance, maxWanderingDistance)), out hit, maxWanderingDistance, NavMesh.AllAreas);

        int i = 0;

        while (Vector3.Distance(transform.position, hit.position) < dectectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(minWanderingDistance, maxWanderingDistance)), out hit, maxWanderingDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    /// <summary>
    /// Attacking ���¿����� �ൿ�� ������Ʈ�մϴ�.
    /// </summary>
    void AttackingUpdate()
    {
        if (playerDistance < attackDistance && IsPlayerInFieldOfView())
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > attackRate)
            {
                lastAttackTime = Time.time;
                CharacterManager.Instance.Player.controller.GetComponent<IDamageable>().TakePhysicalDamage(damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
        else
        {
            if (playerDistance < dectectDistance)
            {
                agent.isStopped = false;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(CharacterManager.Instance.Player.transform.position, path))
                {
                    agent.SetDestination(CharacterManager.Instance.Player.transform.position);
                }
                else
                {
                    agent.SetDestination(transform.position);
                    agent.isStopped = true;
                    SetState(AIState.Wandering);
                }
            }
            else
            {
                agent.SetDestination(transform.position);
                agent.isStopped = true;
                SetState(AIState.Wandering);
            }
        }
    }

    /// <summary>
    /// Fleeing ���¿����� �ൿ�� ������Ʈ�մϴ�.
    /// </summary>
    void FleeingUpdate()
    {
        if (agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else
        {
            SetState(AIState.Wandering);
        }
    }

    /// <summary>
    /// �÷��̾ �þ� ���� �ִ��� Ȯ���մϴ�.
    /// </summary>
    /// <returns>�÷��̾ �þ� ���� �ִ��� ����</returns>
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    /// <summary>
    /// ������ ��ġ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>������ ��ġ</returns>
    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderingDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < safeDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * safeDistance), out hit, maxWanderingDistance, NavMesh.AllAreas);
            i++;
            if (i == 30) break;
        }

        return hit.position;
    }

    /// <summary>
    /// ��ǥ ��ġ���� ������ ��ȯ�մϴ�.
    /// </summary>
    /// <param name="targetPos">��ǥ ��ġ</param>
    /// <returns>����</returns>
    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - CharacterManager.Instance.Player.transform.position, transform.position + targetPos);
    }

    /// <summary>
    /// ������ �������� �޽��ϴ�.
    /// </summary>
    /// <param name="damage">���� ������ ��</param>
    public void TakePhysicalDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
        StartCoroutine(DamageFlash());
    }

    /// <summary>
    /// NPC�� �׾��� �� ȣ��˴ϴ�.
    /// </summary>
    void Die()
    {
        for (int i = 0; i < dropOnDeath.Length; i++)
        {
            Instantiate(dropOnDeath[i].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// �������� ���� �� �÷��� ȿ���� �ݴϴ�.
    /// </summary>
    /// <returns>�ڷ�ƾ</returns>
    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material.color = Color.white;
    }
}
