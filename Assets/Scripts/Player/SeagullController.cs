using Scripts.Inputs;
using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class SeagullController : MonoBehaviour
    {
        public PlayerInputManager Inputs;

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
    
        private Vector3 _currentVelocityRef;
        private Vector2 _moveInput;
        private bool _isSprinting = false;
        private bool _isGrounded = true;
        private bool _isGliding = false;

        void FixedUpdate()
        {
            _moveInput = Inputs.GetMovementDirection();

            switch (_isGrounded)
            {
                case true:
                    WalkMovementMode();
                        break;

                case false:
                    FlightMovementMode();
                    break;
            };

            if (Inputs.GetJumpDown())
                Jump();
            if (Inputs.GetJumpHeld())
                Glide(); 
            else if (_isGliding)
                StopGliding();

            if (Inputs.GetSprintHeld())
                Sprint();
            else if (_isSprinting)
                StopSprinting();

            _anim.SetBool("Idle", _moveInput == Vector2.zero);
        }

        #region Movement Modes
        private void WalkMovementMode()
        {
            // gravity
            _rb.AddForce(Vector3.down * _groundedGravity, ForceMode.Acceleration); 

            float speed = _isSprinting ? _sprintSpeed : _walkSpeed;

            Vector3 movement = Inputs.GetMovementCameraDirection() * speed;

            _rb.linearVelocity = Vector3.SmoothDamp(
                _rb.linearVelocity,
                new Vector3(movement.x, _rb.linearVelocity.y, movement.z),
                ref _currentVelocityRef,
                _smoothTime
            );

            // Rotate seagull to face movement direction
            float targetYaw = movement.magnitude > 0.1f
                ? Quaternion.LookRotation(movement).eulerAngles.y
                : transform.eulerAngles.y;

            Quaternion targetRot = Quaternion.Euler(0, targetYaw, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);

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

                _rb.AddRelativeForce(Vector3.forward * _glideForce, ForceMode.Acceleration); // Fly force
                _rb.AddForce(Vector3.down * _glidingGravity, ForceMode.Acceleration); // gravity
            }
            // If not gliding then they are flying/flapping
            else
            {
                float pitch = _moveInput.y * _flyVerticalRot * Time.deltaTime;
                float yaw = _moveInput.x * _flyHorizontalRot * Time.deltaTime;

                transform.Rotate(pitch, yaw, 0, Space.Self);

                _rb.AddRelativeForce(Vector3.forward * _flyForce, ForceMode.Acceleration); // Fly force
                _rb.AddForce(Vector3.down * _flyingGravity, ForceMode.Acceleration); // gravity
            }

            // Gradually resets the Z-rotation so the gull doesnt get stuck upside down or to the side
            float rollAngle = -_moveInput.x * _maxBankAngle; // how far the gull should rotate
            Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, transform.eulerAngles.y, rollAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);       
        }
        #endregion

        #region On Button Checks
        private void Jump()
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                _rb.AddRelativeForce(2 * _flyForce * Vector3.forward, ForceMode.Acceleration);
            }
        }
        private void Glide()
        {
            _isGliding = true;
            _anim.SetBool("Glide", true);     
        }
        private void StopGliding()
        {
            if (_isGliding)
            {
                _isGliding = false;
                _anim.SetBool("Glide", false);
            }
        }
        private void Sprint()
        {
            _isSprinting = true;
            _anim.SetBool("SprintButton", true);
        }
        private void StopSprinting()
        {
            _isSprinting = false;
            _anim.SetBool("SprintButton", false);
        }
        #endregion

        #region Ground Detection
        private void OnTriggerEnter(Collider other)
        {
            // dont count on collision with another trigger
            if (!other.isTrigger)
            {
                _isGrounded = true;
                _anim.SetBool("Grounded", true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.isTrigger)
            {
                _isGrounded = false;
                _anim.SetBool("Grounded", false);
            }
        }
        #endregion

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
    }
}