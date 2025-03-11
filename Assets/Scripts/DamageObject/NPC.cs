using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// NPC의 AI 상태를 정의하는 열거형
/// </summary>
public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    Fleeing
}

/// <summary>
/// NPC 클래스는 AI 상태에 따라 행동하는 NPC를 정의합니다.
/// </summary>
public class NPC : MonoBehaviour, IDamageable
{
    [Header("Stats")]
    public int health;
    public float walkSpeed;
    public float runSpeed;
    public ItemData[] dropOnDeath; //죽었을 때 드롭 아이템

    [Header("AI")]
    private NavMeshAgent agent;
    public float dectectDistance; //플레이어를 감지하는 거리
    private AIState aiState; //현재 AI 상태

    [Header("Wandering")]
    public float minWanderingDistance; //최소 방황 거리
    public float maxWanderingDistance; //최대 방황 거리
    public float minWanderWaitTime; //최소 방황 대기시간
    public float maxWanderWaitTime; //최대 방황 대기시간
    public float safeDistance; //안전 거리

    [Header("Combat")]
    public int damage;
    public float attackRate;
    private float lastAttackTime; //마지막 공격 시간
    public float attackDistance;

    private float playerDistance; //플레이어와의 거리

    public float fieldOfView = 120f; //시야각

    private Animator animator;
    private SkinnedMeshRenderer[] meshRenderers;

    /// <summary>
    /// 컴포넌트를 초기화합니다.
    /// </summary>
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    /// <summary>
    /// 시작 시 AI 상태를 Wandering으로 설정합니다.
    /// </summary>
    private void Start()
    {
        SetState(AIState.Wandering);
    }

    /// <summary>
    /// 매 프레임마다 AI 상태에 따라 행동을 업데이트합니다.
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
    /// AI 상태를 설정합니다.
    /// </summary>
    /// <param name="state">설정할 AI 상태</param>
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
    /// Idle 또는 Wandering 상태에서의 행동을 업데이트합니다.
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
    /// 새로운 위치로 방황합니다.
    /// </summary>
    void WanderToNewLocation()
    {
        if (aiState != AIState.Idle) return;

        SetState(AIState.Wandering);
        agent.SetDestination(GetWanderLocation());
    }

    /// <summary>
    /// 방황할 위치를 반환합니다.
    /// </summary>
    /// <returns>방황할 위치</returns>
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
    /// Attacking 상태에서의 행동을 업데이트합니다.
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
    /// Fleeing 상태에서의 행동을 업데이트합니다.
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
    /// 플레이어가 시야 내에 있는지 확인합니다.
    /// </summary>
    /// <returns>플레이어가 시야 내에 있는지 여부</returns>
    bool IsPlayerInFieldOfView()
    {
        Vector3 directionToPlayer = CharacterManager.Instance.Player.transform.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < fieldOfView * 0.5f;
    }

    /// <summary>
    /// 도망갈 위치를 반환합니다.
    /// </summary>
    /// <returns>도망갈 위치</returns>
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
    /// 목표 위치와의 각도를 반환합니다.
    /// </summary>
    /// <param name="targetPos">목표 위치</param>
    /// <returns>각도</returns>
    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - CharacterManager.Instance.Player.transform.position, transform.position + targetPos);
    }

    /// <summary>
    /// 물리적 데미지를 받습니다.
    /// </summary>
    /// <param name="damage">받을 데미지 양</param>
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
    /// NPC가 죽었을 때 호출됩니다.
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
    /// 데미지를 받을 때 플래시 효과를 줍니다.
    /// </summary>
    /// <returns>코루틴</returns>
    IEnumerator DamageFlash()
    {
        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int i = 0; i < meshRenderers.Length; i++)
            meshRenderers[i].material.color = Color.white;
    }
}
