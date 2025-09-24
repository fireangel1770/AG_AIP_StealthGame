using UnityEngine;
using UnityEngine.Events;
public class LineOfHearing : MonoBehaviour
{
    [SerializeField] UnityEvent PlayerHeard;
    [SerializeField] PlayerMovement PlayerMovement;

    private void OnTriggerStay(Collider other)
    {
        if (PlayerMovement.isSneaking != true)
        {
            Debug.Log("Herd Player");
            PlayerHeard?.Invoke();
        }
  
    }
}
