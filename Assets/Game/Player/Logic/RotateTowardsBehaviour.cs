using com.editor.GameJamBois.BGJ20201.Attributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace com.runtime.GameJamBois.BGJ20201.Behaviours
{
    public class RotateTowardsBehaviour : MonoBehaviour
    {
        [Header("Basic stuff")]
        [SerializeField] private Transform _targetTransform = default;
        [SerializeField] private bool _isLookAway = true;
        public bool IsRotateAllowed = true;

        [Header("Steps or time")]
        [SerializeField] private bool _isUsingSteps = true;
        [ConditionalHide("_isUsingSteps", true)]
        [SerializeField] private float _stepSizeRadians = 1f;
        [ConditionalHide("_isUsingSteps", true, true)]
        [SerializeField] private float _lerpDurationSeconds = 1f;

        // Update is called once per frame
        void LateUpdate()
        {
            if (IsRotateAllowed)
            {
                Vector3 currentTarget = new Vector3();
                currentTarget.x = transform.position.x - (_isLookAway ? _targetTransform.position.x : -_targetTransform.position.x);
                currentTarget.z = transform.position.z - (_isLookAway ? _targetTransform.position.z : -_targetTransform.position.z);
                if (_isUsingSteps)
                {
                    //moves in fixed steps
                    Vector3 newTarget;
                    newTarget = Vector3.RotateTowards(transform.forward, currentTarget, _stepSizeRadians * Time.deltaTime, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newTarget);
                }
                else
                {
                    //moves via lerp
                    Quaternion newQDirection = Quaternion.LookRotation(currentTarget, Vector3.up);
                    if (_lerpDurationSeconds <= 0)
                    {
                        //the move is instant
                        transform.rotation = newQDirection;
                    }
                    else
                    {
                        //we lerp
                        transform.rotation = Quaternion.Lerp(transform.rotation, newQDirection, 1f * Time.deltaTime / _lerpDurationSeconds);
                    }
                }
            }
        }
    }
}