using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.runtime.GameJamBois.BGJ20201.Util;
using System.Linq;

namespace com.runtime.GameJamBois.BGJ20201.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Physics")]
        [Tooltip("*Required")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _forceMultiplier = 5;
        [Space]

        //TODO: Refactor camera into scriptable object to allow for different cam styles
        [Header("Camera")]
        [Tooltip("*Required")]
        [SerializeField] private Camera _camera;
        [Tooltip("*Required")]
        [SerializeField] private Transform _cameraLockTransform;
        [SerializeField] private float _cameraYMax = 90f;
        [SerializeField] private float _cameraYMin = 0f;
        [SerializeField] private float _cameraXMax = 90f;
        [SerializeField] private float _cameraXMin = 0f;
        [SerializeField] private bool _camRotationSmoothing = true;
        [SerializeField] private Vector3 _camOffset = new Vector3(0, 0, 0);

        private Vector2 _camDirection;
        private Vector2 _playerDirection;

        //TODO: refactor out
        [Header("Mouse")]
        [SerializeField] private bool _rawMouseInput = false;
        [SerializeField] private float _mouseSensitivityX = 1f;
        [SerializeField] private float _mouseSensitivityY = 1f;

        //last mouse position (for FPS)
        private float _rotationX = 0f, _rotationY = 0f;

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
            //cache the current controls
            //TODO: refactor to better control scheme
            _controls = _currentKeys.Keys.ToArray();

            //turn facing
            //transform.rotation = Quaternion.LookRotation(Vector3.forward);

            //set cam at lock position
            //TODO: refactor out, cam stuff
            _camera.transform.position = _cameraLockTransform.position - _camOffset;
            _camDirection = _camera.transform.localRotation.eulerAngles;
            _playerDirection = transform.localRotation.eulerAngles;
        }

        // Update is called once per frame
        void Update()
        {
            //cam
            //TODO: refactor out
            //ripped from https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/
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
            mouseX *= _mouseSensitivityX;
            mouseY *= _mouseSensitivityY;

            _rotationX += mouseX;
            _rotationY += mouseY;

            _rotationX = Mathf.Clamp(_rotationX, _cameraXMin, _cameraXMax);
            _rotationY = Mathf.Clamp(_rotationY, _cameraYMin, _cameraYMax);

            Quaternion qCamDirection = Quaternion.Euler(_camDirection);
            _camera.transform.localRotation = Quaternion.AngleAxis(-_rotationY, qCamDirection * Vector3.right) * qCamDirection;
            _camera.transform.localRotation *= Quaternion.AngleAxis(_rotationX, _camera.transform.InverseTransformDirection(Vector3.up));

            //movement
            _currentKeys = InputUtil.GetIfKeysHeld(_controls);
        }

        private void FixedUpdate()
        {
            SimpleMoveByForce();

            //TODO: refactor cam stuff
            _camera.transform.position = _cameraLockTransform.position - _camOffset;

            //transform.rotation = _camera.transform.rotation;
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

        /// <summary>
        /// Clamps the rotation around x-axis. Taken from the Unity Standard Assets Firsperson Controller (I don't understand Quaternions)
        /// </summary>
        /// <param name="q">The rotation</param>
        /// <returns>The rotation clamped by <see cref="_cameraXMax"/> and <see cref="_cameraXMin"/></returns>
        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            q.x /= q.w;
            q.y /= q.w;
            q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
            angleX = Mathf.Clamp(angleX, _cameraXMin, _cameraXMax);
            q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }
    }
}