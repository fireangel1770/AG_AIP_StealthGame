using UnityEngine;
using UnityEngine.Events;
public class LineOfHearing : MonoBehaviour
{
    [SerializeField] LayerMask playerLayer;
    [SerializeField] UnityEvent PlayerHeard;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        //if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("Herd Player");
            PlayerHeard?.Invoke();
        }
        /*else
        {
            Debug.Log("Herd Something" + other.gameObject.layer);
        }*/
    }
}
