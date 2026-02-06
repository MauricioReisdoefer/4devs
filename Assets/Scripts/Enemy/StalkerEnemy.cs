using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class StalkerEnemy : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform target;

    [Header("Engage")]
    [SerializeField] private float engageDistance = 10f;

    [Header("Flank")]
    [SerializeField] private float behindDistance = 2f;
    [SerializeField] private float updateRate = 0.2f;
    [SerializeField] private float flankStopDistance = 0.3f;

    [Header("Rush")]
    [SerializeField] private float rushSpeedMultiplier = 2.5f;

    private NavMeshAgent agent;
    private float timer;
    private bool isRushing = false;
    private float baseSpeed;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
        agent.avoidancePriority = 30;

        baseSpeed = agent.speed;
    }

    void Update()
    {
        if (!target) return;

        if (Vector2.Distance(transform.position, target.position) > engageDistance)
            return;

        if (isRushing)
        {
            agent.SetDestination(target.position);
            return;
        }
        timer += Time.deltaTime;
        if (timer >= updateRate)
        {
            timer = 0f;

            Vector3 desiredPos;

            if (IsInFrontOfPlayer())
                desiredPos = GetSidePosition();
            else
                desiredPos = GetBehindPosition();

            agent.SetDestination(desiredPos);

            if (!IsInFrontOfPlayer() &&
                !agent.pathPending &&
                agent.remainingDistance <= flankStopDistance)
            {
                StartRush();
            }
        }
    }

    void StartRush()
    {
        isRushing = true;
        agent.speed = baseSpeed * rushSpeedMultiplier;
    }


    Vector3 GetBehindPosition()
    {
        Vector2 forward = GetPlayerForward();
        Vector2 behindDir = -forward;

        return target.position + (Vector3)(behindDir * behindDistance);
    }

    Vector3 GetSidePosition()
    {
        Vector2 forward = GetPlayerForward();

        Vector2 side = new Vector2(-forward.y, forward.x);

        float sign = Mathf.Sign(
            Vector2.Dot(side, (Vector2)transform.position - (Vector2)target.position)
        );

        side *= sign;

        return target.position + (Vector3)(side * behindDistance);
    }

    bool IsInFrontOfPlayer()
    {
        Vector2 forward = GetPlayerForward();
        Vector2 playerToEnemy = ((Vector2)transform.position - (Vector2)target.position).normalized;

        return Vector2.Dot(forward, playerToEnemy) > 0f;
    }

    Vector2 GetPlayerForward()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldMouse = Camera.main.ScreenToWorldPoint(mousePos);
        worldMouse.z = 0f;

        return ((Vector2)worldMouse - (Vector2)target.position).normalized;
    }
}
