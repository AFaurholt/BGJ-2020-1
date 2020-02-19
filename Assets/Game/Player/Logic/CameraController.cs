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
        [SerializeField] private Transform CameraHolder;
        [Space]
        [SerializeField] private bool _hasOneFocus = false;
        [SerializeField] private Transform _cameraPositionFocus = default;
        [ConditionalHide("_hasOneFocus", true, true)]
        [Tooltip("*Not implemented")]
        [SerializeField] private Transform _cameraRotationFocus = default;
        [SerializeField] private bool _cameraIsOrbiting = true;
        [SerializeField] private float _camOffsetDistance = 1f;
        [SerializeField] private Vector3 _camOffsetVector = new Vector3();
        [Space]
        [SerializeField] private bool _hasCamMoveFactorY = false;
        [ConditionalHide("_hasCamMoveFactorY", false)]
        [SerializeField] private float _cameraMoveFactorY = 0.5f;
        [SerializeField] private bool _hasCamMoveFactorX = true;
        [ConditionalHide("_hasCamMoveFactorX", false)]
        [SerializeField] private float _cameraMoveFactorX = 0.5f;
        [Space]
        [SerializeField] private bool _hasCircleCamPan = true;
        [ConditionalHide("_addCircleCamPan", false)]
        [SerializeField] private float _circleAngleMultiplier = 0.15f;
        [ConditionalHide("_addCircleCamPan", false)]
        [SerializeField] private float _circleRadius = 2f;
        //[SerializeField] private bool _camRotationSmoothing = true;
        [Header("Clamping")]
        [SerializeField] private bool _hasRotationY = true;
        [SerializeField] private bool _isYClamped = true;
        [ConditionalHide("_isYClamped", false)]
        [SerializeField] private float _cameraYMax = 90f;
        [ConditionalHide("_isYClamped", false)]
        [SerializeField] private float _cameraYMin = 0f;
        [Space]
        [SerializeField] private bool _hasRotationX = true;
        [SerializeField] private bool _isXClamped = true;
        [ConditionalHide("_isXClamped", false)]
        [SerializeField] private float _cameraXMax = 90f;
        [ConditionalHide("_isXClamped", false)]
        [SerializeField] private float _cameraXMin = 0f;
        [SerializeField] private Vector3 _inputs = Vector3.zero;
        [SerializeField] private float _rotateSpeed = 0f;

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

             if (_cameraIsOrbiting)
            {

                // Changes the CameraHolder's X rotation with mouseInput.y
                Quaternion xRotation = Quaternion.Euler(mouseInput.y, 0f, 0f);
                CameraHolder.rotation = xRotation * CameraHolder.rotation;

                // Changes the CameraHolder's Z rotation with mouseInput.x
                Quaternion zRotation = Quaternion.Euler(0f, 0f, -mouseInput.x);
                CameraHolder.rotation = CameraHolder.rotation * zRotation;

                /* This part kinda screws it up
                float playerRotation = transform.rotation.z;
                float targetRotation = CameraHolder.transform.localRotation.z;
                playerRotation = Mathf.Lerp(playerRotation, targetRotation, _rotateSpeed);
                transform.rotation = Quaternion.Euler(0f, 0f, playerRotation); */
            }
        }
    }
}