using com.runtime.GameJamBois.BGJ20201.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Controllers
{
    public class ManipulateWithMouseController : MonoBehaviour
    {

        private Vector2 _inputs = Vector2.zero;
        private RotationAndPosition _targetRotationAndPosition = new RotationAndPosition(new Quaternion(), new Vector3());

        [Header("Mouse")]
        [SerializeField] private Transform _parentTransform = default;
        [SerializeField] private bool _ignoreParentRotate = true;
        [SerializeField] private bool _rawMouseInput = false;
        [SerializeField] private float _mouseSensitivityX = 1f;
        [SerializeField] private float _mouseSensitivityY = 1f;
        [Space]
        [SerializeField] private List<MouseBehaviour> _editorMouseBehaviourX = new List<MouseBehaviour>();
        [SerializeField] private List<MouseBehaviour> _editorMouseBehaviourY = new List<MouseBehaviour>();
        private HashSet<MouseBehaviour> _mouseBehaviourX = new HashSet<MouseBehaviour>();
        private HashSet<MouseBehaviour> _mouseBehaviourY = new HashSet<MouseBehaviour>();
        [Space]
        [SerializeField] private bool _useAlternateScheme = false;
        public Vector3 XVector3 = new Vector3();
        public Vector3 YVector3 = new Vector3();
        [Space]
        [Tooltip("Not implemented")]
        [SerializeField] private Transform _orbitTarget = default;
        [Space]
        [SerializeField] private bool _isClampingX = false;
        [SerializeField] private float _clampMaxX = 0f;
        [SerializeField] private float _clampMinX = 0f;
        [Space]
        [SerializeField] private bool _isClampingY = false;
        [SerializeField] private float _clampMaxY = 0f;
        [SerializeField] private float _clampMinY = 0f;
        [Space]
        [SerializeField] private bool _invertY = false;
        [SerializeField] private bool _invertX = false;
        [Space]
        [SerializeField] private bool _lockCursor = false;
        [SerializeField] private bool _cursorVisible = false;

        private Vector2 _mouseInput = Vector2.zero;

        private HashSet<MouseBehaviour> _orbitBehaviourSet = new HashSet<MouseBehaviour> {
            MouseBehaviour.OrbitX,
            MouseBehaviour.OrbitY,
            MouseBehaviour.OrbitZ };
        void Start()
        {
            //cursor settings
            if (_lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (_cursorVisible)
            {
                Cursor.visible = false;
            }

            //convert to hashset
            ConvertListToHashSet(ref _mouseBehaviourX, _editorMouseBehaviourX);
            ConvertListToHashSet(ref _mouseBehaviourY, _editorMouseBehaviourY);

            //orbit check
            if (_orbitTarget != null)
            {
                Debug.LogError("_orbitTarget is not implemented");
            }
            if (HasOrbitBehaviouer() && _orbitTarget == null)
            {
                Debug.LogError("_orbitTarget cannot be null with orbit behaviour");
            }

            //set initial values
            if (_parentTransform != null)
            {
                _targetRotationAndPosition.position = _parentTransform.position - transform.position;
                _targetRotationAndPosition.rotation = _parentTransform.rotation * transform.rotation;
            }
            else
            {
                _targetRotationAndPosition.position = transform.position;
                _targetRotationAndPosition.rotation = transform.rotation;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (_rawMouseInput)
            {
                _inputs.x = Input.GetAxisRaw("Mouse X");
                _inputs.y = Input.GetAxisRaw("Mouse Y");
            }
            else
            {
                _inputs.x = Input.GetAxis("Mouse X");
                _inputs.y = Input.GetAxis("Mouse Y");
            }

            Vector2 mouseInput = new Vector2(
                (_invertX ? -_inputs.x : _inputs.x) * _mouseSensitivityX,
                (_invertY ? -_inputs.y : _inputs.y) * _mouseSensitivityY);

            ClampAndStoreMouseInput(mouseInput);

            if (_useAlternateScheme)
            {
                Vector3 euler = _mouseInput.x * XVector3 + _mouseInput.y * YVector3;
                _targetRotationAndPosition.rotation.eulerAngles = euler;
            }
            else
            {
                ConvertMouseInputToRotationAndPosition(_mouseInput);
            }

            if (!HasOrbitBehaviouer())
            {
                if (_parentTransform != null)
                {
                    transform.SetPositionAndRotation(
                        _parentTransform.position - _targetRotationAndPosition.position,
                        (_ignoreParentRotate ? _targetRotationAndPosition.rotation : _parentTransform.rotation * _targetRotationAndPosition.rotation));
                }
                else
                {
                    transform.SetPositionAndRotation(_targetRotationAndPosition.position, _targetRotationAndPosition.rotation);
                }
            }
        }
        /// <summary>
        /// Assumes the values in <see cref="MouseBehaviour"/> are ordered X, Y, Z
        /// </summary>
        /// <returns>True if either <see cref="_mouseBehaviourX"/> or <see cref="_mouseBehaviourY"/> does an orbit</returns>
        bool HasOrbitBehaviouer()
        {
            return _mouseBehaviourX.Overlaps(_orbitBehaviourSet) || _mouseBehaviourY.Overlaps(_orbitBehaviourSet);
        }

        void ConvertListToHashSet<T>(ref HashSet<T> hashSet, List<T> list)
        {
            foreach (var item in list)
            {
                hashSet.Add(item);
            }
        }

        void ClampAndStoreMouseInput(Vector2 input)
        {
            _mouseInput += input;

            if (_isClampingX)
            {
                _mouseInput.x = Mathf.Clamp(_mouseInput.x, _clampMinX, _clampMaxX);
            }
            if (_isClampingY)
            {
                _mouseInput.y = Mathf.Clamp(_mouseInput.y, _clampMinY, _clampMaxY);
            }
        }

        void ConvertMouseInputToRotationAndPosition(Vector2 mouseInput)
        {
            for (int i = 0; i < 2; i++)
            {
                HashSet<MouseBehaviour> currentSet = i == 0 ? _mouseBehaviourX : _mouseBehaviourY;
                foreach (MouseBehaviour behaviour in currentSet)
                {
                    switch (behaviour)
                    {
                        case MouseBehaviour.None:
                            break;
                        case MouseBehaviour.PositionX:
                            _targetRotationAndPosition.position.x = mouseInput[i];
                            break;
                        case MouseBehaviour.PositionY:
                            _targetRotationAndPosition.position.y = mouseInput[i];
                            break;
                        case MouseBehaviour.PositionZ:
                            _targetRotationAndPosition.position.z = mouseInput[i];
                            break;
                        case MouseBehaviour.RotateX:
                            {
                                Vector3 euler = _targetRotationAndPosition.rotation.eulerAngles;
                                euler.x = mouseInput[i];
                                _targetRotationAndPosition.rotation.eulerAngles = euler;
                                break;
                            }
                        case MouseBehaviour.RotateY:
                            {
                                Vector3 euler = _targetRotationAndPosition.rotation.eulerAngles;
                                euler.y = mouseInput[i];
                                _targetRotationAndPosition.rotation.eulerAngles = euler;
                                break;
                            }
                        case MouseBehaviour.RotateZ:
                            {
                                Vector3 euler = _targetRotationAndPosition.rotation.eulerAngles;
                                euler.z = mouseInput[i];
                                _targetRotationAndPosition.rotation.eulerAngles = euler;
                                break;
                            }
                        case MouseBehaviour.OrbitX:
                            break;
                        case MouseBehaviour.OrbitY:
                            break;
                        case MouseBehaviour.OrbitZ:
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        enum MouseBehaviour
        {
            None,
            PositionX,
            PositionY,
            PositionZ,
            RotateX,
            RotateY,
            RotateZ,
            OrbitX,
            OrbitY,
            OrbitZ,
        }
    }
}