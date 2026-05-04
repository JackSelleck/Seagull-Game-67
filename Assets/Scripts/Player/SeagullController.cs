using Scripts.Inputs;
using UnityEngine;

namespace Scripts.Player
{
    [DisallowMultipleComponent]
    public class SeagullController : MonoBehaviour
    {
        [SerializeField] private PlayerStats _playerStats;
        [SerializeField] private PlayerInputManager _inputs;
        [SerializeField] private Animator _anim;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private CapsuleCollider _col;

        // flat flight
        private bool _forceFlatFlight = false;
        private float _flatFlightMinY;

        private Vector3 _currentVelocityRef;
        private Vector2 _moveInput;
        private bool _navigateOverObstacle = false;
        private bool _isSprinting = false;
        private bool _isGrounded = false;
        private bool _isGliding = false;
        private float _glideAccelTimer;

        private void Awake()
        {
            if (_inputs == null)
                _inputs = GetComponent<PlayerInputManager>();
        }

        void FixedUpdate()
        {
            _moveInput = _inputs.GetMovementDirection();

            if (_isGrounded)
            {
                WalkMovementMode();
            }
            else
            {
                FlightMovementMode();
            }

            if (_inputs.GetJumpDown())
                Jump();

            SetSimpleState(ref _isGliding, PlayerAnimHash.Glide, _inputs.GetJumpHeld());
            SetSimpleState(ref _isSprinting, PlayerAnimHash.SprintButton, _inputs.GetSprintHeld());

            _anim.SetBool(PlayerAnimHash.Idle, _moveInput == Vector2.zero);
        }

        #region Movement Modes
        private void WalkMovementMode()
        {
            // gravity
            _rb.AddForce(Vector3.down * _playerStats.groundedGravity, ForceMode.Acceleration); 

            float speed = _isSprinting ? _playerStats.sprintSpeed : _playerStats.walkSpeed;

            Vector3 movement = _inputs.GetMovementCameraDirection() * speed;

            _rb.linearVelocity = Vector3.SmoothDamp(
                _rb.linearVelocity,
                new Vector3(movement.x, _rb.linearVelocity.y, movement.z),
                ref _currentVelocityRef,
                _playerStats.smoothTime
            );

            // Rotate seagull to face movement direction
            float targetYaw = movement.magnitude > 0.1f
                ? Quaternion.LookRotation(movement).eulerAngles.y
                : transform.eulerAngles.y;

            Quaternion targetRot = Quaternion.Euler(0, targetYaw, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);

            _anim.SetBool(PlayerAnimHash.Moving, movement.magnitude > 0.1f);
        }

        private void FlightMovementMode()
        {
            // Get correct flight values based on current flight mode
            float verticalRot = _isGliding ? _playerStats.glideModeVerticalRot : _playerStats.flapModeVerticalRot;
            float horizontalRot = _isGliding ? _playerStats.glideModeHorizontalRot : _playerStats.flapModeHorizontalRot;
            float gravity = _forceFlatFlight ? 0 : _isGliding ? _playerStats.glidingModeGravity : _playerStats.flapModeGravity;
            float maxBankAngle = _forceFlatFlight ? 25f : _playerStats.maxBankAngle;

            float moveForce;
            if (_isGliding)
            {
                moveForce = AcellerateGlide();
            }
            else
            {
                _glideAccelTimer = 0f;
                moveForce = _playerStats.flapModeForce;
            }            

            float pitch = _moveInput.y * verticalRot * Time.deltaTime;
            float yaw = _moveInput.x * horizontalRot * Time.deltaTime;
            transform.Rotate(pitch, yaw, 0, Space.Self);

            Vector3 velocity = _rb.linearVelocity;
            Vector3 euler = transform.eulerAngles;

            // Movement
            _rb.AddRelativeForce(Vector3.forward * moveForce, ForceMode.Acceleration);
            _rb.AddForce(Vector3.down * gravity, ForceMode.Acceleration);
            _anim.SetFloat(PlayerAnimHash.Speed, velocity.magnitude);

            // Gull z rotation
            // also gradually resets the Z-rotation so the gull doesnt get stuck upside down or to the side
            float rollAngle = -_moveInput.x * maxBankAngle; // how far the gull should rotate
            Quaternion targetRotation = Quaternion.Euler(euler.x, euler.y, rollAngle);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 2f);

            if (FindFlightBlockings())
            {
                // Makes the gull navigate over the obstacle
                _rb.AddForce(10f * Vector3.up, ForceMode.Acceleration);
                _rb.linearVelocity = new Vector3(Vector3.up.x, Vector3.up.y * 1.5f, Vector3.up.z);
                // Makes the gull freak out and messes up your controls as response to hitting an obstacle
                _rb.angularVelocity = new Vector3(transform.rotation.x, 90f, transform.rotation.z);
                // Rotate to face the sky during freak out, helps ensure the player doesnt get stuck
                Quaternion _navigationRotation = Quaternion.Euler(-90f, euler.y, euler.z);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _navigationRotation, Time.deltaTime * 10000f);
            }
            // Flight mode for when close to the floor
            else if (_forceFlatFlight) 
            {
                // limit downward rotation
                // Normalise euler angles to be -180/180, rather than 0-360 to simplify down lock
                float normalisedPitch = euler.x;
                normalisedPitch = Mathf.DeltaAngle(0f, normalisedPitch);
                if (normalisedPitch > 0f)
                {
                    transform.eulerAngles = new Vector3(0f, euler.y, euler.z);
                    Vector3 newAngle = _rb.angularVelocity;
                    if (newAngle.x > 0f) _rb.angularVelocity = new Vector3(0f, newAngle.y, newAngle.z);
                }

                // smoothly push seagull up so it dosent go too close to the ground
                float dip = _flatFlightMinY - _rb.position.y;
                _rb.AddForce(100f * Mathf.Max(dip, 0f) * Vector3.up, ForceMode.Acceleration);

                if (_rb.linearVelocity.y < 0f)
                {
                    _rb.linearVelocity = new Vector3(velocity.x, velocity.y * 0.85f, velocity.z);
                }
            }
        }
        private bool FindFlightBlockings()
        {
            Vector3 origin = transform.position;
            Vector3 forwardDir = transform.forward;
            forwardDir.y = 0f;
            forwardDir.Normalize();

            // check if there is an obstacle in front
            bool isBlocked = Physics.Raycast(origin, forwardDir, out RaycastHit forwardHit, _playerStats.forwardBlockCheckDist);

            if (isBlocked && !forwardHit.collider.CompareTag(TagConstants.NPC) && !forwardHit.collider.CompareTag(TagConstants.NoGroundingZone))
            {
                // check if there is space above
                return _navigateOverObstacle = !Physics.Raycast(origin, Vector3.up, 100);
            }
            else
            {
                return _navigateOverObstacle = false;
            }
        }
        private float AcellerateGlide()
        {
            _glideAccelTimer += Time.fixedDeltaTime;
            float t = Mathf.Clamp01(_glideAccelTimer / _playerStats.glideModeVelocityTime);
            float targetForce = _playerStats.glideModeForce + _playerStats.glideModeMaxVelocity;
            return Mathf.Lerp(_playerStats.glideModeForce, targetForce, t);
        }
        #endregion

        #region On Button Checks
        private void Jump()
        {
            if (_isGrounded)
            {
                _rb.AddForce(Vector3.up * _playerStats.jumpForce, ForceMode.Impulse);
                _rb.AddRelativeForce(2 * _playerStats.flapModeForce * Vector3.forward, ForceMode.Acceleration);
            }
        }
        // Helper to change states that are just a bool and animation change
        private void SetSimpleState(ref bool current, int animParamHash, bool desired)
        {
            // If bool being passed matches what its supposed to be then return
            if (current == desired) return;
            // Otherwise update state to match
            current = desired;
            _anim.SetBool(animParamHash, desired);
        }
        #endregion

        #region Ground Detection
        private void OnTriggerEnter(Collider other)
        {
            // dont count on collision with another trigger
            if (other.CompareTag(TagConstants.NoGroundingZone))
            {
                _forceFlatFlight = true;
                _flatFlightMinY = other.bounds.min.y;
                return;
            }
            else if (!other.isTrigger && !_navigateOverObstacle)
            {
                _isGrounded = true;
                _anim.SetBool(PlayerAnimHash.Grounded, true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(TagConstants.NoGroundingZone))
            {
                _forceFlatFlight = false;
            }
            if (!other.isTrigger)
            {
                _isGrounded = false;
                _anim.SetBool(PlayerAnimHash.Grounded, false);
            }
        }
        #endregion
    }
}