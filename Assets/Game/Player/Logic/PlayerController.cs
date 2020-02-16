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
            _camera.transform.position = _cameraLockTransform.position;

        }

        // Update is called once per frame
        void Update()
        {
            //cam
            //TODO: refactor out
            //TODO: add clamp for Y
            //TODO: add clamp for X for staggered turn (no owl head)
            //TODO: add sensitivity
            _rotationX = _camera.transform.localEulerAngles.y + Input.GetAxis("Mouse X");
            _rotationY += Input.GetAxis("Mouse Y");

            _camera.transform.localEulerAngles = new Vector3(-_rotationY, _rotationX, 0);

            //movement
            _currentKeys = InputUtil.GetIfKeysHeld(_controls);
        }

        private void FixedUpdate()
        {
            SimpleMoveByForce();

            //TODO: refactor cam stuff
            _camera.transform.position = _cameraLockTransform.position;
            transform.rotation = _camera.transform.rotation;
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