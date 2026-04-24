using Scripts.Player;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Inputs
{
    [DisallowMultipleComponent]
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset _actions;
        [SerializeField] private PlayerReferenceManager _playerRefs;

        // Player actions
        private InputAction _move;
        private InputAction _look;
        private InputAction _attack;
        private InputAction _interact;
        private InputAction _crouch;
        private InputAction _jump;
        private InputAction _previous;
        private InputAction _next;
        private InputAction _sprint;

        // UI controls
        private InputAction _navigate;
        private InputAction _submit;
        private InputAction _cancel;
        private InputAction _point;
        private InputAction _click;
        private InputAction _scrollWheel;
        private InputAction _middleClick;
        private InputAction _rightClick;
        private InputAction _trackedDevicePosition;
        private InputAction _trackedDeviceOrientation;

        readonly private List<string> UIActions = new()
        {
           "Navigate", "Submit", "Cancel", "Point",
           "Click", "ScrollWheel", "MiddleClick", "RightClick",
           "TrackedDevicePosition", "TrackedDeviceOrientation"
        };

        private float? _lastJumpTime;
        private const string _mouseDeviceName = "Mouse";
        private const float _jumpBuffer = 0.15f;

        private void CacheActions()
        {
            _move = _actions["Move"];
            _look = _actions["Look"];
            _attack = _actions["Attack"];
            _interact = _actions["Interact"];
            _crouch = _actions["Crouch"];
            _jump = _actions["Jump"];
            _previous = _actions["Previous"];
            _next = _actions["Next"];
            _sprint = _actions["Sprint"];

            _navigate = _actions["Navigate"];
            _submit = _actions["Submit"];
            _cancel = _actions["Cancel"];
            _point = _actions["Point"];
            _click = _actions["Click"];
            _scrollWheel = _actions["ScrollWheel"];
            _middleClick = _actions["MiddleClick"];
            _rightClick = _actions["RightClick"];
            _trackedDevicePosition = _actions["TrackedDevicePosition"];
            _trackedDeviceOrientation = _actions["TrackedDeviceOrientation"];
        }

        public Vector2 GetMovementDirection()
        {
            var value = _move.ReadValue<Vector2>();
            return value;
        }

        public Vector3 GetMovementCameraDirection(bool localSpace = true)
        {
            var input = GetMovementDirection();
            var direction = new Vector3(input.x, 0, input.y);

            if (direction.sqrMagnitude > 0)
            {
                var rotation = Quaternion.FromToRotation(_playerRefs.Cam.transform.up, transform.up);
                direction = rotation * _playerRefs.Cam.transform.rotation * direction;

                if (localSpace)
                {
                    direction = Vector3.ProjectOnPlane(direction, transform.up);
                    direction = Quaternion.FromToRotation(transform.up, Vector3.up) * direction;
                }
                direction = direction.normalized;
            }

            return direction;
        }

        public bool IsLookingWithMouse()
        {
            if (_look.activeControl == null)
            {
                return false;
            }

            return _look.activeControl.device.name.Equals(_mouseDeviceName);
        }

        // not that useful rn but I might apply a buffer like this to a few spammable moves
        public bool GetJumpDown()
        {
            if (_lastJumpTime != null &&
                Time.time - _lastJumpTime < _jumpBuffer)
            {
                _lastJumpTime = null;
                return true;
            }

            return false;
        }
        public bool GetAttackDown() => _attack.WasPressedThisFrame();
        public bool GetAttackHeld() => _attack.IsPressed();
        public bool GetInteractDown() => _interact.WasPressedThisFrame();
        public bool GetCrouchHeld() => _crouch.IsPressed();
        public bool GetJumpUp() => _jump.WasReleasedThisFrame();
        public bool GetJumpHeld() => _jump.IsPressed();
        public bool GetPreviousDown() => _previous.WasPressedThisFrame();
        public bool GetNextDown() => _next.WasPressedThisFrame();
        public bool GetSprintHeld() => _sprint.IsPressed();

        public bool EscPressed()
        {
#if UNITY_STANDALONE
            return Keyboard.current.escapeKey.wasPressedThisFrame;
#else
			return false;
#endif
        }

        private void Awake()
        {
            if (_playerRefs == null)
                _playerRefs = GetComponent<PlayerReferenceManager>();

            CacheActions();
        }

        private void Start()
        {
            _actions.Enable();
        }

        private void Update()
        {
            if (_jump.WasPressedThisFrame())
            {
                _lastJumpTime = Time.time;
            }
        }

        /// <summary>
        /// Disable all inputs
        /// </summary>
        public void SetNoControls(bool shouldNotMove)
        {
            if (shouldNotMove)
            {
                foreach (var action in _actions)
                {
                   action.Disable();
                }
            }
            else
            {
                // enable everything again
                _actions.Enable();
            }
        }

        public void SetUIControls(bool inUI)
        {
            if (inUI)
            {
                foreach (var action in _actions)
                {
                    if (UIActions.Contains(action.name))
                        action.Enable();
                    else
                        action.Disable();
                }
            }
            else
            {
                // enable everything again
                _actions.Enable();
            }
        }

        private void OnEnable() => _actions.Enable();
        private void OnDisable() => _actions.Disable();
    }
}