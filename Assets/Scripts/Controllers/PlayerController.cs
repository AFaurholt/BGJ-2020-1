using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.runtime.GameJamBois.BGJ20201.Util;
using com.editor.GameJamBois.BGJ20201.Attributes;
using System.Linq;

namespace com.runtime.GameJamBois.BGJ20201.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Physics")]
        [Tooltip("*Required")]
        [SerializeField] private Rigidbody _rigidbody = default;
        [SerializeField] private float _forceMultiplier = 5;
        [Space]

        //TODO: Refactor camera into scriptable object to allow for different cam styles
        [Header("Camera")]
        [Tooltip("*Required")]
        [SerializeField] private Camera _camera = default;
        [Tooltip("*Required")]
        [SerializeField] private bool _hasOneFocus = false;
        [SerializeField] private Transform _cameraPositionFocus = default;
        [ConditionalHide("_hasOneFocus", true, true)]
        [Tooltip("*Not implemented")]
        [SerializeField] private Transform _cameraRotationFocus = default;
        [SerializeField] private bool _cameraIsOrbiting = true;
        [SerializeField] private float _camOffsetDistance = 1f;
        [SerializeField] private Vector3 _camOffsetVector = new Vector3();
        //[SerializeField] private bool _camRotationSmoothing = true;
        [Header("Clamping")]
        [SerializeField] private bool _isYClamped = true;
        [ConditionalHide("_isYClamped", true)]
        [SerializeField] private float _cameraYMax = 90f;
        [ConditionalHide("_isYClamped", true)]
        [SerializeField] private float _cameraYMin = 0f;
        [Space]
        [SerializeField] private bool _isXClamped = true;
        [ConditionalHide("_isXClamped", true)]
        [SerializeField] private float _cameraXMax = 90f;
        [ConditionalHide("_isXClamped", true)]
        [SerializeField] private float _cameraXMin = 0f;

        //TODO: refactor out
        [Header("Mouse")]
        [SerializeField] private bool _rawMouseInput = false;
        [SerializeField] private float _mouseSensitivityX = 1f;
        [SerializeField] private float _mouseSensitivityY = 1f;

        private Vector2 _camTargetRotations = new Vector2(0f, 0f);

        //the currently held keys
        //TODO: refactor better input system
        private Dictionary<KeyCode, bool> _currentKeys = new Dictionary<KeyCode, bool>()
        {
            { KeyCode.W, false },
            { KeyCode.A, false },
            { KeyCode.S, false },
            { KeyCode.D, false },
        };

        private KeyCode[] _controls;

        // Start is called before the first frame update
        void Start()
        {
            if (_hasOneFocus)
            {
                _cameraRotationFocus = _cameraPositionFocus;
            }

            //cache the current controls
            //TODO: refactor to better control scheme
            _controls = _currentKeys.Keys.ToArray();
        }

        // Update is called once per frame
        void Update()
        {
            //cam
            //TODO: refactor out
            //ripped from https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/
            //and https://catlikecoding.com/unity/tutorials/movement/orbit-camera/
            float mouseY, mouseX;
            if (_rawMouseInput)
            {
                mouseX = Input.GetAxisRaw("Mouse X");
                mouseY = Input.GetAxisRaw("Mouse Y");
            }
            else
            {
                mouseX = Input.GetAxis("Mouse X");
                mouseY = Input.GetAxis("Mouse Y");
            }

            if (_camOffsetDistance < 0)
            {
                mouseY *= -1;
            }
            Vector2 mouseInput = new Vector2(mouseY * _mouseSensitivityY,
            mouseX * _mouseSensitivityX);

            _camTargetRotations += mouseInput;
            if (_isYClamped)
            {
                _camTargetRotations.x = Mathf.Clamp(_camTargetRotations.x, _cameraYMin, _cameraYMax);
            }
            if (_isXClamped)
            {
                _camTargetRotations.y = Mathf.Clamp(_camTargetRotations.y, _cameraXMin, _cameraXMax);
            }

            if (_cameraIsOrbiting)
            {
                Quaternion lookRotation = Quaternion.Euler(_camTargetRotations);
                Vector3 lookDirection = lookRotation * Vector3.forward;
                Vector3 lookPosition = _cameraPositionFocus.position - (lookDirection * _camOffsetDistance) - _camOffsetVector;
                _camera.transform.SetPositionAndRotation(lookPosition, lookRotation);
            }
            else
            {
                //I'm aware you're not supposed to edit the rotation directly, but the camera is not going to use physics
                _camera.transform.rotation = Quaternion.Euler(-_camTargetRotations.y, _camTargetRotations.x, 0);
            }

            //movement
            _currentKeys = InputUtil.GetIfKeysHeld(_controls);
        }

        //TODO refactor some cam stuff out and put it in lateupdate
        //private void LateUpdate()
        //{
            
        //}
        private void FixedUpdate()
        {
            SimpleMoveByForce();

            //TODO: refactor cam stuff
            if (!_cameraIsOrbiting)
            {
                _camera.transform.position = _cameraPositionFocus.position - _camOffsetVector;
            }

        }
        /// <summary>
        /// Simple <see cref="UnityEngine.Rigidbody.AddRelativeForce(Vector3, ForceMode)"/> based on facing. Reads controls from <see cref="_currentKeys"/>
        /// </summary>
        void SimpleMoveByForce()
        {
            foreach (var item in _currentKeys)
            {
                if (item.Value)
                {
                    switch (item.Key)
                    {
                        case KeyCode.A:
                            _rigidbody.AddRelativeForce(Vector3.left * _forceMultiplier, ForceMode.Impulse);
                            break;
                        case KeyCode.D:
                            _rigidbody.AddRelativeForce(Vector3.right * _forceMultiplier, ForceMode.Impulse);
                            break;
                        case KeyCode.S:
                            _rigidbody.AddRelativeForce(Vector3.back * _forceMultiplier, ForceMode.Impulse);
                            break;
                        case KeyCode.W:
                            _rigidbody.AddRelativeForce(Vector3.forward * _forceMultiplier, ForceMode.Impulse);
                            break;
                    }
                }
            }

        }
    }
}