using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(LineRenderer))]
public class RangedFollower : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Ranges")]
    [SerializeField] private float attackRange = 8f;

    [Header("Shooting")]
    [SerializeField] private float fireCooldown = 1.2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float laserDuration = 0.1f;

    [Header("Vision")]
    [SerializeField] private LayerMask visionMask;

    [Header("Attack Visual")]
    [SerializeField] private float attackChargeTime = 0.25f;

    private NavMeshAgent agent;
    private LineRenderer line;

    private float fireTimer;

    private SpriteRenderer sprite;
    private Color startColor;

    private Coroutine attackRoutine;
    private float attackTimer;

    private void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        startColor = sprite.color;
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;

        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.enabled = false;
        line.useWorldSpace = true;
        line.startWidth = 0.05f;
        line.endWidth = 0.05f;
        line.material = new Material(Shader.Find("Sprites/Default"));
    }

    private void Update()
    {
        if (!target) return;

        float distToPlayer = Vector2.Distance(transform.position, target.position);
        if (distToPlayer > 14f)
            return;

        float distance = Vector2.Distance(transform.position, target.position);

        if (!HasLineOfSight() || distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.position);
            CancelAttackRoutine();
            fireTimer = 0f;
            return;
        }

        agent.isStopped = true;
        HandleShooting();
    }

    void HandleShooting()
    {
        fireTimer += Time.deltaTime;

        if (attackRoutine == null)
        {
            attackRoutine = StartCoroutine(AttackChargeRoutine());
        }

        if (fireTimer >= fireCooldown)
        {
            fireTimer = 0f;
            ShootRay();
        }
    }

    IEnumerator AttackChargeRoutine()
    {
        attackTimer = 0f;

        while (attackTimer < attackChargeTime)
        {
            attackTimer += Time.deltaTime;
            float t = attackTimer / attackChargeTime;
            sprite.color = Color.Lerp(startColor, Color.white, t);
            yield return null;
        }
    }

    void CancelAttackRoutine()
    {
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        sprite.color = startColor;
        attackTimer = 0f;
    }

    void ShootRay()
    {
        Vector2 origin = transform.position;
        Vector2 direction = (target.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, attackRange, visionMask);

        Vector2 endPoint = origin + direction * attackRange;

        if (hit)
        {
            endPoint = hit.point;

            if (hit.transform.CompareTag("Player"))
            {
                hit.transform
                    .GetComponent<IHealthComponent>()
                    ?.SufferDamange(damage);
            }
        }

        StartCoroutine(ShowLaser(origin, endPoint));

        CancelAttackRoutine();
    }

    IEnumerator ShowLaser(Vector2 start, Vector2 end)
    {
        line.enabled = true;
        line.SetPosition(0, start);
        line.SetPosition(1, end);

        yield return new WaitForSeconds(laserDuration);

        line.enabled = false;
    }

    bool HasLineOfSight()
    {
        Vector2 origin = transform.position;
        Vector2 dir = (target.position - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(origin, dir, attackRange, visionMask);
        return hit && hit.transform == target;
    }
}
