using UnityEngine;
using UnityEngine.Events;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] Transform target;
    EvilJeffStates state;

    [SerializeField] LayerMask environmentLayer;
    public UnityEvent SawPlayer;

    [SerializeField] float sightRadius = 0.25f; 
    void Update()
    {
        switch (state)
        {
            case EvilJeffStates.IDEL:
                break;
            case EvilJeffStates.CHASE:
                break;
            case EvilJeffStates.WONDER:
                break;
            case EvilJeffStates.INVESTUGATE:
                break;
        }
        Vector3 directionToTarget = (target.position - transform.position);
        directionToTarget.y = 0;
        directionToTarget.Normalize(); 
        Vector3 forwardDirection = transform.forward;

        float dot = Vector3.Dot(forwardDirection, directionToTarget);
        
        if (dot > sightRadius)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToTarget, out hit, 10000, environmentLayer))
            {
                Debug.DrawLine(transform.position, target.position, Color.magenta);
                Debug.Log("Saw a wall");
            }
            else
            {
                Debug.DrawLine(transform.position, target.position, Color.green);
                Debug.Log("Saw the Player");
                SawPlayer?.Invoke();
            }
        }
        else //(dot < 0.5f)
        {
            Debug.DrawLine(transform.position, target.position, Color.red);
            Debug.Log("I Saw Nothing");
        }
    }
}
