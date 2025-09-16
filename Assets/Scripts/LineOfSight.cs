using UnityEngine;
using UnityEngine.Events;

public class LineOfSight : MonoBehaviour
{
    [SerializeField] Transform targer;
    EvilJeffStates state;

    [SerializeField] LayerMask enviromentLayer;
    public UnityEvent SawPlayer;
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
        Vector3 directionToTargetn = (transform.position - targer.position).normalized;
        Vector3 forwardDirection = transform.forward;

        float dot = Vector3.Dot(forwardDirection, directionToTargetn);
        
        if (dot > 0.25f)
        {
            Debug.DrawLine(transform.position, targer.position, Color.red);
            Debug.Log("Behind the Enemy");
        }
        else //(dot < 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, targer.position, out hit, 1000, enviromentLayer))
            {
                Debug.DrawLine(transform.position, targer.position, Color.red);
                Debug.Log("Saw a wall");
                SawPlayer?.Invoke();
            }
            else
            {
                Debug.DrawLine(transform.position, targer.position, Color.green);
                Debug.Log("Saw the Player");
            }
        }
    }
}
