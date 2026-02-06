using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFollower : MonoBehaviour
{
    [SerializeField] private Transform target;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateUpAxis = false;
        agent.updateRotation = false;
    }
    private void Update()
    {
        

        if (target)
        {
            float distToPlayer = Vector2.Distance(transform.position, target.position);
            if (distToPlayer > 12f)
                return;
            agent.SetDestination(target.position);
        }
    }
}
