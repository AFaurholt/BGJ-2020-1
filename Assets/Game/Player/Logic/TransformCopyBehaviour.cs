﻿using com.editor.GameJamBois.BGJ20201.Attributes;
using com.runtime.GameJamBois.BGJ20201.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.runtime.GameJamBois.BGJ20201.Behaviours
{
    public class TransformCopyBehaviour : MonoBehaviour
    {
        public bool UseInRecordMode = false;
        public Transform CopyTransform = default;
        [Space]
        public bool IsCopyPosition = true;
        public bool HasSmoothingPosition = true;
        [ConditionalHide("HasSmoothingPosition", false)]
        public float SmoothTimePosition = 0.1f;
        private Vector3 _positionVelocity = Vector3.zero;
        [Space]
        public bool IsCopyRotation = true;
        public bool HasSmoothingRotation = true;
        public bool UseLerpInsteadOfRotateTowards = true;
        [Tooltip("Angles per second")]
        public float RotationSpeed = 0.2f;
        [Space]
        public Vector3 CameraOffsetVector3 = new Vector3();

        private RotationAndPosition _rotationAndPosition = new RotationAndPosition(new Quaternion(), new Vector3());

        private void Start()
        {
            _rotationAndPosition.position = transform.position;
            _rotationAndPosition.rotation = transform.rotation;
        }

        private void FixedUpdate()
        {
            if (IsCopyPosition)
            {
                Vector3 newPos =
                    HasSmoothingPosition ?
                    Vector3.SmoothDamp(_rotationAndPosition.position, CopyTransform.position, ref _positionVelocity, SmoothTimePosition) :
                    CopyTransform.position;
                _rotationAndPosition.position = newPos - CameraOffsetVector3;
            }
            if (IsCopyRotation)
            {
                if (UseLerpInsteadOfRotateTowards)
                {
                    Quaternion newRot = HasSmoothingRotation ?
                    Quaternion.Lerp(_rotationAndPosition.rotation, CopyTransform.rotation, RotationSpeed * Time.fixedDeltaTime) :
                    CopyTransform.rotation;

                    // Makes it only rotate on one axis
                    Vector3 rotEuler = newRot.eulerAngles;
                    rotEuler.x = 0;
                    newRot = Quaternion.Euler(rotEuler);

                    _rotationAndPosition.rotation = newRot;
                }
                else
                {
                    Quaternion newRot = HasSmoothingRotation ?
                    Quaternion.RotateTowards(_rotationAndPosition.rotation, CopyTransform.rotation, RotationSpeed * Time.fixedDeltaTime) :
                    CopyTransform.rotation;
                    _rotationAndPosition.rotation = newRot;
                }
            }
            if (!UseInRecordMode)
            {
                transform.position = _rotationAndPosition.position;
                transform.rotation = _rotationAndPosition.rotation;
            }
        }
    }
}