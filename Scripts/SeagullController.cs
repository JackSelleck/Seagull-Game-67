using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SeagullController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;
    [Header("Movement")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float groundedGravity = 2f;
    [SerializeField] private float flyingGravity = 0.5f;
    [SerializeField] private float glidingGravity = 0.2f;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;

    private InputSystem_Actions controls;

    private bool isGrounded = false;
    private bool isGliding = false;
    private Vector2 moveInput;
    private Vector3 currentVelocityRef; 


    private void Awake()
    {
        controls = new InputSystem_Actions();
        controls.Player.Move.performed += ctx => Move();
        controls.Player.Jump.performed += ctx => OnSpaceButton(ctx);
        controls.Player.Jump.canceled += ctx => StopGliding(ctx);
    }

    void Start()
    {
        
    }

    void FixedUpdate()
    {
        ApplyGravity();
            
        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if (moveInput != Vector2.zero)
        {
            Vector3 movement = new Vector3(moveInput.x, 0, moveInput.y);
            rb.linearVelocity = Vector3.SmoothDamp(
                rb.linearVelocity,
                new Vector3(movement.x, rb.linearVelocity.y, movement.z),
                ref currentVelocityRef,
                smoothTime
            ); rb.constraints = RigidbodyConstraints.FreezeRotationX| RigidbodyConstraints.FreezeRotationZ;
            anim.SetBool("Moving", true);
            Debug.Log("Moving");

            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(-moveInput.x, 0, -moveInput.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        else if (isGrounded)
        {
            rb.linearVelocity = new Vector3(0, 0, 0);
            anim.SetBool("Moving", false);
        }
    }

    private void ApplyGravity()
    {
        if (!isGrounded && !isGliding)
        {
            rb.AddForce(Vector3.down * flyingGravity, ForceMode.Acceleration);
        }
        else if (isGliding)
        {
            rb.AddForce(Vector3.down * glidingGravity, ForceMode.Acceleration);
        }
        else if (isGrounded)
        {
            rb.AddForce(Vector3.down * groundedGravity, ForceMode.Acceleration);
        }
    }

    private void Move()
    {

    }
    private void OnSpaceButton(InputAction.CallbackContext context)
    {
        if (context.interaction is TapInteraction)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (context.interaction is HoldInteraction)
        {
            isGliding = true;
            anim.SetBool("Glide", true);
        }
        Debug.Log("Jumped!");
    }

    private void StopGliding(InputAction.CallbackContext context)
    {
        if (isGliding)
        {
            isGliding = false;
            anim.SetBool("Glide", false);
            Debug.Log("Space Released: GLIDE STOOOOOOOOOOP!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Grounded", true);
    }
    private void OnCollisionExit(Collision collision)
    {
        isGrounded = true;
        anim.SetBool("Grounded", false);
    }

    private void OnEnable()
    {
        controls.Enable();
    }
    private void OnDisable()
    {
        controls.Disable();
    }
}
