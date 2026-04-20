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
        [SerializeField] private Transform _cam;
        [Space]
        [Header("Flight Tweaks")]
        [SerializeField] private float _smoothTime = 0.2f;
        [SerializeField] private float _maxBankAngle = 80;
        [Header("Fly")]
        [SerializeField] private float _flyForce = 10;
        [SerializeField] private float _flyVerticalRot = 100;
        [SerializeField] private float _flyHorizontalRot = 100;
        [SerializeField] private float _flyingGravity = 2;
        [Header("Glide")]
        [SerializeField] private float _glideForce = 15;
        [SerializeField] private float _glideVerticalRot = 50;
        [SerializeField] private float _glideHorizontalRot = 50;
        [SerializeField] private float _glidingGravity = 6;
        [Header("Walk Tweaks")]
        [SerializeField] private float _groundedGravity = 7;
        [SerializeField] private float _walkSpeed = 1.67f;
        [SerializeField] private float _sprintSpeed = 2;
        [SerializeField] private float _jumpForce = 1;
    
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
            _controls.Player.Sprint.performed += _ => OnSprintButton();
            _controls.Player.Sprint.canceled += _ => OnSprintButtonRelease();
            _controls.Player.Jump.performed += ctx => OnJumpButton(ctx);
            _controls.Player.Jump.canceled += ctx => StopGliding(ctx);
        }
    
        void FixedUpdate()
        {
            _moveInput = _controls.Player.Move.ReadValue<Vector2>();

            switch (_isGrounded)
            {
                case true:
                    WalkMovementMode();
                        break;

                case false:
                    FlightMovementMode();
                    break;
            };

            _anim.SetBool("Idle", _moveInput == Vector2.zero);
        }

        #region Movement Modes
        private void WalkMovementMode()
        {
            _rb.AddForce(Vector3.down * _groundedGravity, ForceMode.Acceleration);

            float speed = _isSprinting ? _sprintSpeed : _walkSpeed;

            // project on plane makes walking ignore camera y position
            Vector3 camForward = Vector3.ProjectOnPlane(_cam.forward, Vector3.up).normalized;
            Vector3 camRight = Vector3.ProjectOnPlane(_cam.right, Vector3.up).normalized;
            Vector3 movement = (camRight * _moveInput.x + camForward * _moveInput.y) * speed;

            _rb.linearVelocity = Vector3.SmoothDamp(
                _rb.linearVelocity,
                new Vector3(movement.x, _rb.linearVelocity.y, movement.z),
                ref _currentVelocityRef,
                _smoothTime
            );

            // Rotate seagull to face movement direction
            if (movement.magnitude > 0.1f)
            {
                Quaternion targetRot = Quaternion.LookRotation(movement);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            _anim.SetBool("Moving", movement.magnitude > 0.1f);
        }
        private void FlightMovementMode()
        {
            Vector3 velocity = new(_rb.linearVelocity.x, _rb.linearVelocity.y, _rb.linearVelocity.z);
            _anim.SetFloat("Speed", velocity.magnitude);

            if (_isGliding)
            {
                float pitch = _moveInput.y * _glideVerticalRot * Time.deltaTime;
                float yaw = _moveInput.x * _glideHorizontalRot * Time.deltaTime;

                transform.Rotate(pitch, yaw, 0, Space.Self);

                _rb.AddRelativeForce(Vector3.forward * _glideForce, ForceMode.Acceleration);
                _rb.AddForce(Vector3.down * _glidingGravity, ForceMode.Acceleration);
            }
            // If not gliding then they are flying/flapping
            else
            {
                float pitch = _moveInput.y * _flyVerticalRot * Time.deltaTime;
                float yaw = _moveInput.x * _flyHorizontalRot * Time.deltaTime;

                transform.Rotate(pitch, yaw, 0, Space.Self);

                _rb.AddRelativeForce(Vector3.forward * _flyForce, ForceMode.Acceleration);
                _rb.AddForce(Vector3.down * _flyingGravity, ForceMode.Acceleration);
            }

            // Gradually resets the Z-rotation so the gull dont get stuck upside down
            float rollAngle = -_moveInput.x * _maxBankAngle; // how far the gull should visually rotate
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rollAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);       
        }

        #region On Button Checks
        private void OnJumpButton(InputAction.CallbackContext context)
        {
            if (context.interaction is TapInteraction)
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _rb.AddRelativeForce(2 * _flyForce * Vector3.forward, ForceMode.Acceleration);
            }
            if (context.interaction is HoldInteraction)
            {
                _isGliding = true;
                _anim.SetBool("Glide", true);
            }
        }
        private void OnSprintButton()
        {
            _isSprinting = true;
            _anim.SetBool("SprintButton", true);
        }
        private void OnSprintButtonRelease()
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
            }
        }
        #endregion

        #region Ground Detection
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
    }
}