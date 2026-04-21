using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Scripts.Inputs
{
    [DisallowMultipleComponent]
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private InputActionAsset actions;
        [SerializeField] private Camera _cam;

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
            _move = actions["Move"];
            _look = actions["Look"];
            _attack = actions["Attack"];
            _interact = actions["Interact"];
            _crouch = actions["Crouch"];
            _jump = actions["Jump"];
            _previous = actions["Previous"];
            _next = actions["Next"];
            _sprint = actions["Sprint"];

            _navigate = actions["Navigate"];
            _submit = actions["Submit"];
            _cancel = actions["Cancel"];
            _point = actions["Point"];
            _click = actions["Click"];
            _scrollWheel = actions["ScrollWheel"];
            _middleClick = actions["MiddleClick"];
            _rightClick = actions["RightClick"];
            _trackedDevicePosition = actions["TrackedDevicePosition"];
            _trackedDeviceOrientation = actions["TrackedDeviceOrientation"];
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
                var rotation = Quaternion.FromToRotation(_cam.transform.up, transform.up);
                direction = rotation * _cam.transform.rotation * direction;

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

        private void Awake() => CacheActions();

        private void Start()
        {
            actions.Enable();
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
                foreach (var action in actions)
                {
                   action.Disable();
                }
            }
            else
            {
                // enable everything again
                actions.Enable();
            }
        }

        public void SetUIControls(bool inUI)
        {
            if (inUI)
            {
                foreach (var action in actions)
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
                actions.Enable();
            }
        }

        private void OnEnable() => actions.Enable();
        private void OnDisable() => actions.Disable();
    }
}