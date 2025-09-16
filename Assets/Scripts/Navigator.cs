using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
public enum EvilJeffStates
{
    IDEL,
    WONDER,
    CHASE,
    INVESTUGATE
}
public class Navigator : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    Rigidbody rb;
    [SerializeField] float speed;
    NavMeshPath navPath;
    Queue<Vector3> remainingPoints;
    Vector3 currentTargetPoint;

    [SerializeField] EvilJeffStates state = EvilJeffStates.IDEL;

    [SerializeField] Transform[] patrolPoints;
    Transform currentPatrolPoint;
    int patrolPointIndex;

    float elapsed = 0; 
    bool agiantStop = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();

        currentPatrolPoint = patrolPoints[0];
    }
    void Update()
    {
        if (agiantStop == true)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
            switch (state)
            {
                case EvilJeffStates.IDEL:
                    UpdateIdel();
                    break;
                case EvilJeffStates.CHASE:
                    UpdateChase();
                    break;
                case EvilJeffStates.WONDER:
                    UpdateWonder();
                    break;
                case EvilJeffStates.INVESTUGATE:
                    UpdateInvestugate();
                    break;
            }
      
    }
    void UpdateIdel()
    {
        agiantStop = true;
        elapsed += Time.deltaTime;
        if (elapsed > 3.0f)
        {
            elapsed = 0;
        }
        if (elapsed >= 2)
        {
            elapsed = 0;
            state = EvilJeffStates.WONDER;
        }
    }
    void UpdateWonder()
    {
        agiantStop = false;
        agent.SetDestination(currentPatrolPoint.position);
        float distance = Vector3.Distance(transform.position, currentPatrolPoint.position);
        if (distance < 2)
        {
            patrolPointIndex++;
            if (patrolPointIndex >= patrolPoints.Length)
            {
                patrolPointIndex = 0;
            }
            currentPatrolPoint = patrolPoints[patrolPointIndex];
            agent.SetDestination(currentPatrolPoint.position);
            state = EvilJeffStates.IDEL;
        }
        Debug.DrawLine(transform.position, currentPatrolPoint.position, Color.yellow);
    }
    void UpdateChase()
    {
        agiantStop = true;
        elapsed += Time.deltaTime;
        if (elapsed > 5)
        {
            if (agent.CalculatePath(target.position, navPath))
            {
                remainingPoints.Clear();
                Debug.Log("Found a path to target");
                foreach (Vector3 p in navPath.corners)
                {
                    remainingPoints.Enqueue(p);
                }
                currentTargetPoint = remainingPoints.Dequeue();
            }
            elapsed = 0;
        }
        Vector3 new_Foward = (currentTargetPoint - transform.position).normalized;
        new_Foward.y = 0;
        transform.forward = new_Foward;
        float distToPoint = Vector3.Distance(transform.position, currentTargetPoint);

        if (distToPoint < 2)
        {
            currentTargetPoint = remainingPoints.Dequeue();
        }
    }
    void UpdateInvestugate()
    {
        if (agent.CalculatePath(target.position, navPath))
        {
            remainingPoints.Clear();
            Debug.Log("Found a path to target");
            foreach (Vector3 p in navPath.corners)
            {
                remainingPoints.Enqueue(p);
            }
            currentTargetPoint = remainingPoints.Dequeue();
        }
    }
    private void FixedUpdate()
    {
        switch (state)
        {
            case EvilJeffStates.IDEL:
                rb.linearVelocity = Vector3.zero;
                break;
            case EvilJeffStates.CHASE:
                rb.linearVelocity = transform.forward * speed;
                break;
            case EvilJeffStates.WONDER:
                Vector3 direction = currentPatrolPoint.position - transform.position;
                direction = direction.normalized;
                break;
            case EvilJeffStates.INVESTUGATE:
                rb.linearVelocity = transform.forward * speed;
                break;
        }
    }
    private void OnDrawGizmos()
    {
        if (navPath == null)
            return;
        //Debug.Log("Is Gizmoing");
        foreach (Vector3 node in navPath.corners)
        {
            //Debug.Log("Is painting");
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(node, 1);
        }
    }
}
