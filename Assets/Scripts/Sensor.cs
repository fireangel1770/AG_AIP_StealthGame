using UnityEngine;
using UnityEngine.Events;
public class Sensor : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider> OnHeard;

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Something Entered Triger " + other.gameObject.name);
        OnHeard?.Invoke(other);
    }
}
