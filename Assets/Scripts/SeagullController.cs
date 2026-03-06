using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class SeagullController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private CapsuleCollider col;
    [Space]
    [Header("Flight Tweaks")]
    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private float flyforce = 10;
    [SerializeField] private float glideforce = 8;
    [SerializeField] private float pitchSpeed = 8;
    [SerializeField] private float turnSpeed = 8;
    [SerializeField] private float maxBankAngle = 8;
    [Space]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float jumpForce = 5f;
    [Space]
    [SerializeField] private float groundedGravity = 2f;
    [SerializeField] private float flyingGravity = 0.5f;
    [SerializeField] private float glidingGravity = 0.2f;

    private InputSystem_Actions controls;

    private bool isGrounded = true;
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
        ApplyAmbientForces();
        Vector3 velocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y, rb.linearVelocity.z);
        anim.SetFloat("Speed", velocity.magnitude);

        moveInput = controls.Player.Move.ReadValue<Vector2>();

        if (isGrounded)
        {
            Vector3 movement = new Vector3(-moveInput.x, 0, -moveInput.y) * walkSpeed;

            rb.linearVelocity = Vector3.SmoothDamp(
                 rb.linearVelocity,
                 new Vector3(movement.x, rb.linearVelocity.y, movement.z),
                 ref currentVelocityRef,
                 smoothTime
            );
            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }

            anim.SetBool("Moving", movement.magnitude > 0.1f);
        }
        else 
        {
            float pitch = moveInput.y * pitchSpeed * Time.deltaTime;
            float yaw = moveInput.x * turnSpeed * Time.deltaTime;

            transform.Rotate(pitch, yaw, 0, Space.Self);

            if (isGliding)
            {
                rb.AddRelativeForce(Vector3.forward * glideforce, ForceMode.Acceleration);
                rb.AddForce(Vector3.down * glidingGravity, ForceMode.Acceleration);
                Debug.Log("Flying");
            }
            else if (!isGliding)
            {
                rb.AddRelativeForce(Vector3.forward * flyforce, ForceMode.Acceleration);
                rb.AddForce(Vector3.down * flyingGravity, ForceMode.Acceleration);
                anim.SetBool("Gliding", true);
                Debug.Log("Gliding");
            }

            // Gradually resets the Z-rotation so the gull dont get stuck upside down
            float rollAngle = -moveInput.x * maxBankAngle; // Visual banking
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rollAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

            rb.constraints = RigidbodyConstraints.None; 
            rb.linearDamping = 4f;       
        }

        if (moveInput == Vector2.zero)
        {
            anim.SetBool("Idle", true);
        }
        else {anim.SetBool("Idle", false);}
    }

    private void ApplyAmbientForces()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.down * groundedGravity, ForceMode.Acceleration);
            Debug.Log("Grounded");
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
            rb.AddRelativeForce(Vector3.forward * flyforce * 2, ForceMode.Acceleration);
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

    private void OnTriggerEnter(Collider other)
    {
        isGrounded = true;
        anim.SetBool("Grounded", true);
    }
    private void OnTriggerExit(Collider other)
    {
        isGrounded = false;
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
