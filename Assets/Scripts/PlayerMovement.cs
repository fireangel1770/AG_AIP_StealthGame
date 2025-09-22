using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] InputAction playerController;
    Vector2 moveInput;

    [SerializeField] float speed;
    public bool isSneaking;
    public bool isSprinting;

    [SerializeField] Transform cameraTransform;
    Vector3 MovingVector;
    Rigidbody rb;

    Vector3 forward;
    Vector3 right;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        isSneaking = false;
        isSprinting = false;
    }
// Update is called once per frame
void Update()
    {
        

    }

    public void OnMove(InputValue v)
    {
        moveInput = v.Get<Vector2>();
        MovingVector = new Vector3(moveInput.x, 0, moveInput.y);

        
    }
    public void OnCrouch()
    {
        isSneaking = !isSneaking;
    }
    public void OnSprint()
    {
        isSprinting = !isSprinting;
    }
    private void FixedUpdate()
    {
        var forward = cameraTransform.transform.forward;
        var right = cameraTransform.transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * MovingVector.z + right * MovingVector.x;

        MovingVector = desiredMoveDirection;

        if (isSneaking)
        {
            //transform.Translate(desiredMoveDirection * speed * Time.deltaTime);
            rb.AddForce(MovingVector * (speed * 0.5f), ForceMode.Acceleration);
        }
        else if (isSprinting)
        {
            //transform.Translate(desiredMoveDirection * speed * Time.deltaTime);
            rb.AddForce(MovingVector * (speed * 1.5f), ForceMode.Acceleration);
        }
        else
        {
            //transform.Translate(desiredMoveDirection * speed * Time.deltaTime);
            rb.AddForce(MovingVector * speed, ForceMode.Acceleration);
        }
    }
}
