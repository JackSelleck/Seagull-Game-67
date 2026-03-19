using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace SeagullMovementSystem
{
    public class SeagullController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Animator _anim;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private CapsuleCollider _col;
        [Space]
        [Header("Flight Tweaks")]
        [SerializeField] private float _smoothTime = 0.2f;
        [SerializeField] private float _maxBankAngle = 8;
        [Header("Fly")]
        [SerializeField] private float _flyforce = 50;
        [SerializeField] private float _flyVerticalRot = 100;
        [SerializeField] private float _flyHorizontalRot = 100;
        [SerializeField] private float _flyingGravity = 2f;
        [Header("Glide")]
        [SerializeField] private float _glideforce = 80;
        [SerializeField] private float _glideVerticalRot = 50;
        [SerializeField] private float _glideHorizontalRot = 50;
        [SerializeField] private float _glidingGravity = 15f;
        [Header("Walk Tweaks")]
        [SerializeField] private float _groundedGravity = 2f;
        [SerializeField] private float _walkSpeed = 5f;
        [SerializeField] private float _sprintSpeed = 10f;
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _groundCheckDistance = 0.3f;
        [SerializeField] private LayerMask _groundLayer;
    
        private InputSystem_Actions _controls;
        private Vector3 _currentVelocityRef;
        private Vector2 _moveInput;
        private bool _patrolFlightMode = true;
        private bool _isSprinting = false;
        private bool _isGrounded = true;
        private bool _isGliding = false;

        private void Awake()
        {
            _controls = new InputSystem_Actions();
            _controls.Player.Jump.performed += ctx => OnJumpButton(ctx);
            _controls.Player.Sprint.performed += ctx => OnSprintButton(ctx);
            _controls.Player.Sprint.canceled += ctx => OnSprintButtonRelease(ctx);
            // allow glide whilst space or other jump button held
            _controls.Player.Jump.canceled += ctx => StopGliding(ctx);
        }
    
        void FixedUpdate()
        {
            ApplyAmbientForces();
    
            if (_patrolFlightMode)
            {
                PatrolFlightMode();
            }
            else { PrecisionFlightMode();}

            if (_moveInput == Vector2.zero)
            {
                _anim.SetBool("Idle", true);
            }
            else {_anim.SetBool("Idle", false);}
        }

        #region Flight
        private void PatrolFlightMode()
        {
            Vector3 velocity = new(_rb.linearVelocity.x, _rb.linearVelocity.y, _rb.linearVelocity.z);
            _anim.SetFloat("Speed", velocity.magnitude);

            _moveInput = _controls.Player.Move.ReadValue<Vector2>();

            // If grounded then walk
            if (_isGrounded)
            {
                Vector3 movement;

                if (_isSprinting)
                {
                    movement = new Vector3(-_moveInput.x, 0, -_moveInput.y) * _sprintSpeed;
                }
                else
                {
                    movement = new Vector3(-_moveInput.x, 0, -_moveInput.y) * _walkSpeed;
                }

                _rb.linearVelocity = Vector3.SmoothDamp(
                         _rb.linearVelocity,
                         new Vector3(movement.x, _rb.linearVelocity.y, movement.z),
                         ref _currentVelocityRef,
                         _smoothTime
                    );
                if (movement.magnitude > 0.1f)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(new Vector3(movement.x, 0, movement.z));
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                _anim.SetBool("Moving", movement.magnitude > 0.1f);
            }
            // If not grounded then check if they should flap or glide
            else
            {
                if (_isGliding)
                {
                    float pitch = _moveInput.y * _glideVerticalRot * Time.deltaTime;
                    float yaw = _moveInput.x * _glideHorizontalRot * Time.deltaTime;

                    transform.Rotate(pitch, yaw, 0, Space.Self);

                    _rb.AddRelativeForce(Vector3.forward * _glideforce, ForceMode.Acceleration);
                    _rb.AddForce(Vector3.down * _glidingGravity, ForceMode.Acceleration);
                    //Debug.Log("Gliding");
                }
                // If not gliding then they are flying/flapping
                else
                {
                    float pitch = _moveInput.y * _flyVerticalRot * Time.deltaTime;
                    float yaw = _moveInput.x * _flyHorizontalRot * Time.deltaTime;

                    transform.Rotate(pitch, yaw, 0, Space.Self);

                    _rb.AddRelativeForce(Vector3.forward * _flyforce, ForceMode.Acceleration);
                    _rb.AddForce(Vector3.down * _flyingGravity, ForceMode.Acceleration);
                    _anim.SetBool("Gliding", false);
                    //Debug.Log("Flying");
                }

                // Gradually resets the Z-rotation so the gull dont get stuck upside down
                float rollAngle = -_moveInput.x * _maxBankAngle; // how far the gull should visually rotate
                Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rollAngle);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);
            }
        }
        /// <summary>
        /// Precision mode:
        /// Easier to control than the patrol flight mode, as to make stealing more feasible
        /// Should not allow you to touch the ground unless you crash into something
        /// Should have a dive move with a bit of an aim assist
        /// </summary>
        private void PrecisionFlightMode()
        {


            Debug.Log("EnteredPrecisionFlightMode");
        }
        #endregion

        private void ApplyAmbientForces()
        {
            // Grounded gravity
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.down * _groundedGravity, ForceMode.Acceleration);
                Debug.Log("Grounded");
            }
        }

        #region On Button Checks
        private void OnJumpButton(InputAction.CallbackContext context)
        {
            if (context.interaction is TapInteraction)
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _rb.AddRelativeForce(2 * _flyforce * Vector3.forward, ForceMode.Acceleration);
            }
            if (context.interaction is HoldInteraction)
            {
                _isGliding = true;
                _anim.SetBool("Glide", true);
            }
            Debug.Log("Jumped!");
        }
        private void OnSprintButton(InputAction.CallbackContext context)
        {
            _isSprinting = true;
            _anim.SetBool("SprintButton", true);
        }
        private void OnSprintButtonRelease(InputAction.CallbackContext context)
        {
            _isSprinting = false;
            _anim.SetBool("SprintButton", false);
        }
        private void StopGliding(InputAction.CallbackContext _)
        {
            if (_isGliding)
            {
                _isGliding = false;
                _anim.SetBool("Glide", false);
                Debug.Log("Space Released: GLIDE STOOOOOOOOOOP!");
            }
        }
        #endregion

        #region Ground Detection
        private bool CheckGrounded()
        {
            return Physics.CheckSphere(
                _col.bounds.center - Vector3.up * _col.bounds.extents.y,
                _groundCheckDistance,
                _groundLayer
            );
        }
        private void OnTriggerEnter(Collider other)
        {
            _isGrounded = true;
            _anim.SetBool("Grounded", true);
        }
        private void OnTriggerExit(Collider other)
        {
            _isGrounded = false;
            _anim.SetBool("Grounded", false);
        }
        #endregion

        private void OnEnable()
        {
            _controls.Enable();
        }
        private void OnDisable()
        {
            _controls.Disable();
        }
    }
}