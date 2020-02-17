using com.editor.GameJamBois.BGJ20201.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera")]
        [Tooltip("*Required")]
        [SerializeField] private Camera _camera = default;
        [Space]
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
        [SerializeField] private Vector3 _inputs = Vector3.zero;

        //TODO: refactor out
        [Header("Mouse")]
        [SerializeField] private bool _rawMouseInput = false;
        [SerializeField] private float _mouseSensitivityX = 1f;
        [SerializeField] private float _mouseSensitivityY = 1f;
        [SerializeField] private bool _invertY = false;
        [SerializeField] private bool _invertX = true;
        [Space]
        [SerializeField] private bool _lockCursor = false;
        [SerializeField] private bool _cursorVisible = false;

        private Vector2 _camTargetRotations = new Vector2(0f, 0f);
        // Start is called before the first frame update
        void Start()
        {
            if (_hasOneFocus)
            {
                _cameraRotationFocus = _cameraPositionFocus;
            }
            //cursor settings
            if (_lockCursor)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (_cursorVisible)
            {
                Cursor.visible = false;
            }
        }

        // Update is called once per frame
        void Update()
        {
            //ripped from https://forum.unity.com/threads/a-free-simple-smooth-mouselook.73117/
            //and https://catlikecoding.com/unity/tutorials/movement/orbit-camera/
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

            if (_camOffsetDistance < 0)
            {
                _inputs.y *= -1;
            }

            Vector2 mouseInput = new Vector2(_inputs.x * _mouseSensitivityX,
            _inputs.y * _mouseSensitivityY);

            _camTargetRotations.x += mouseInput.x;
            _camTargetRotations.y += mouseInput.y;

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
                Quaternion lookRotation = Quaternion.Euler(
                    (_invertY ? -_camTargetRotations.y : _camTargetRotations.y), 
                    (_invertX ? -_camTargetRotations.x : _camTargetRotations.x),
                    0);
                Vector3 lookPosition = _cameraPositionFocus.position - (lookRotation * Vector3.forward * _camOffsetDistance) - _camOffsetVector;
                _camera.transform.SetPositionAndRotation(lookPosition, lookRotation);
            }
            else
            {
                //I'm aware you're not supposed to edit the rotation directly, but the camera is not going to use physics
                _camera.transform.rotation = Quaternion.Euler(-_camTargetRotations.y, _camTargetRotations.x, 0);
            }
        }

        private void FixedUpdate()
        {
            if (!_cameraIsOrbiting)
            {
                _camera.transform.position = _cameraPositionFocus.position - _camOffsetVector;
            }
        }
    }
}