using System;
using System.IO;
using UnityEngine;
using UnityEngine.AI;

public class Navigator : MonoBehaviour
{ 
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] Transform[] patrolPoints;
    int patrolPointIndex;
    NavMeshPath path;
    Transform currentPatrolPoint;

    Rigidbody rb;
    [SerializeField] float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        NavMeshPath path = new NavMeshPath();
  
    }
    
    // Update is called once per frame
    void Update()
    {
        currentPatrolPoint = patrolPoints[patrolPointIndex];
        agent.SetDestination(currentPatrolPoint,)
        if (agent.CalculatePath(currentPatrolPoint.positon, path))
        {

        }
        else
        {
            Debug.Log("Path Fail");
        }

        float distance = Vector3.Distance(transform.position,currentPatrolPoint.position);
    }
    private void FixedUpdate()
    {
        Vector3 direction = currentPatrolPoint.position - transform.position;
        direction = direction.normalized;
        rb.linearVelocity = direction * moveSpeed;
    }
    private void OnDrawGizmos()
    {
        if (path == null)
            return;
        foreach (var corner in path.corners)
        {   
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(corner, 1);
        }
    }
    public void HeardSomething(Collider thingWeHeard)
    {
        Debug.Log("Did you hear that??");
    }
    public void SawSomething(Collider thingWeSaw)
    {
        Vector3 guardForward = transform.forward;
        Vector3 lineToBob;

    }

}
