using System;
using System.Collections.Generic;
using System.Collections;
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
    public bool agentStop = false;

    [SerializeField] float distanceThreshold = 2;
    [SerializeField] bool isInvestigating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        navPath = new NavMeshPath();
        remainingPoints = new Queue<Vector3>();

        currentPatrolPoint = patrolPoints[0];
    }
    void Update()
    {
        if (agentStop == true)
        {
            agent.isStopped = true;
        }
        else
        {
            agent.isStopped = false;
        }
        elapsed += Time.deltaTime;

        switch (state)
        {
            case EvilJeffStates.IDEL:
                UpdateIdle();
                break;
            case EvilJeffStates.CHASE:
                UpdateChase();
                break;
            case EvilJeffStates.WONDER:
                UpdateWonder();
                break;
            case EvilJeffStates.INVESTUGATE:
                UpdateInvestigate();
                break;
        }

    }
    void UpdateIdle()
    {
        agentStop = true;
        if (elapsed >= 2)
        {
            elapsed = 0;
            state = EvilJeffStates.WONDER;
        }
    }
    void UpdateWonder()
    {
        agentStop = false;
        agent.SetDestination(currentPatrolPoint.position);
        if (elapsed > 2)
        {
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
            elapsed = 0;
        }
        Debug.DrawLine(transform.position, currentPatrolPoint.position, Color.yellow);
    }
    void UpdateChase()
    {
        //agentStop = true;
        //    if (agent.CalculatePath(target.position, navPath))
        //    {
        //        remainingPoints.Clear();
        //        Debug.Log("Found a path to target");
        //        foreach (Vector3 p in navPath.corners)
        //        {
        //            remainingPoints.Enqueue(p);
        //        }
        //        currentTargetPoint = remainingPoints.Dequeue();
        //    }

        //Vector3 new_Forward = (currentTargetPoint - transform.position).normalized;
        //new_Forward.y = 0;
        //transform.forward = new_Forward;
        //float distanceToPoint = Vector3.Distance(transform.position, currentTargetPoint);

        //if (distanceToPoint < distanceThreshold)
        //{
        //    currentTargetPoint = remainingPoints.Dequeue();
        //}
    }
    void UpdateInvestigate()
    {
        agentStop = true;
        if (agent.CalculatePath(target.position, navPath))
        {
            remainingPoints.Clear();
            Debug.Log("Found a path to target");
            foreach (Vector3 p in navPath.corners)
            {
                remainingPoints.Enqueue(p);
            }
            currentTargetPoint = remainingPoints.Dequeue();
            currentTargetPoint = remainingPoints.Dequeue();
        }

        Vector3 new_Forward = (currentTargetPoint - transform.position).normalized;
        new_Forward.y = 0;
        transform.forward = new_Forward;
        float distToPoint = Vector3.Distance(transform.position, currentTargetPoint);

        if (remainingPoints.Count == 0)
        {
            state = EvilJeffStates.IDEL;
            elapsed = 0;
            return;
        }

        if (distToPoint < distanceThreshold)
        {
            currentTargetPoint = remainingPoints.Dequeue();
        }
    }
    IEnumerator TimeInvestigate()
    {
        if (isInvestigating == true)
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
                currentTargetPoint = remainingPoints.Dequeue();
            }

            Vector3 new_Forward = (currentTargetPoint - transform.position).normalized;
            new_Forward.y = 0;
            transform.forward = new_Forward;
            float distToPoint = Vector3.Distance(transform.position, currentTargetPoint);

            if (distToPoint < distanceThreshold)
            {
                currentTargetPoint = remainingPoints.Dequeue();
            }
        }
        yield return 0;
    } 
    public void PlayerDetected()
    {
        state = EvilJeffStates.INVESTUGATE;
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
