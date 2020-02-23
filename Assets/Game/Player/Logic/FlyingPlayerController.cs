using com.runtime.GameJamBois.BGJ20201.Util;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class FlyingPlayerController : MonoBehaviour
{
    [Header("Forces")]
    public Vector3 GravityVector3 = new Vector3(0f, 0f, 1f);
    public float MoveForce = 5f;

    [Header("Directions")]
    public Vector3 UpVector3 = Vector3.up;
    public Vector3 RightVector3 = Vector3.right;
    [Header("Rotations")]
    public Transform CopyTransform = default;
    public bool MoveRelativeToRotation = true;
    [Space]

    private Rigidbody _rb;
    private Vector3 _internalVelocity = Vector3.zero;
    private Vector3 _currentMoveVector = new Vector3();
    private Quaternion _internalRotation = new Quaternion();
    [SerializeField] private float _smoothTime = 0.1f;

    [SerializeField] Animator anim;
    [SerializeField] float animSpeed;

    //the currently held keys
    //TODO: refactor better input system
    private IDictionary<KeyCode, bool> _currentKeys = new Dictionary<KeyCode, bool>()
        {
            { KeyCode.W, false },
            { KeyCode.A, false },
            { KeyCode.S, false },
            { KeyCode.D, false },
        };

    private KeyCode[] _controls;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();

        //cache the current controls
        //TODO: refactor to better control scheme
        _controls = _currentKeys.Keys.ToArray();

        if (MoveRelativeToRotation)
        {
            _internalRotation = CopyTransform.localRotation;
        }
    }
    private void Update()
    {
        //movement
        _currentKeys = InputUtil.GetIfKeysHeld(_controls);

        UpdateAnimator();
        
        Vector3 targetMoveVector = GetMovementVector(_currentKeys, MoveForce);
        _currentMoveVector = Vector3.SmoothDamp(_currentMoveVector, targetMoveVector, ref _internalVelocity, _smoothTime);
        targetMoveVector = GravityVector3 + _currentMoveVector;

        OriginShifter.MoveOriginBy(targetMoveVector * Time.deltaTime);
        

    }
    private void FixedUpdate()
    {
        /*
        Vector3 targetMoveVector = GetMovementVector(_currentKeys, MoveForce);
        _currentMoveVector = Vector3.SmoothDamp(_currentMoveVector, targetMoveVector, ref _internalVelocity, _smoothTime);
        targetMoveVector = GravityVector3 + _currentMoveVector;

        OriginShifter.MoveOriginBy(targetMoveVector * Time.fixedDeltaTime);
        */
    }

    void UpdateAnimator()
    {
        float x = 0;
        float y = 0;
        if(anim == null)
        {
            Debug.LogError("boi gotta set that animator");
            return;
        }
        foreach (var i in _currentKeys)
        {
            if(i.Value)
            {
                switch (i.Key)
                {
                    case KeyCode.W:
                        y += 1;
                        break;
                    case KeyCode.A:
                        x -= 1;
                        break;
                    case KeyCode.S:
                        y -= 1;
                        break;
                    case KeyCode.D:
                        x += 1;
                        break;
                }
            }
        }

        anim.SetFloat("X", Mathf.Lerp(anim.GetFloat("X"), x, animSpeed*Time.deltaTime));
        anim.SetFloat("Y", Mathf.Lerp(anim.GetFloat("Y"), y, animSpeed*Time.deltaTime));
    }

    /// <summary>
    /// Gets the movement vector3 based on current keys. Only supports WASD controls
    /// </summary>
    Vector3 GetMovementVector(IDictionary<KeyCode, bool> currentKeys, float movementForce)
    {
        UpdateInternalRotation();
        Vector3 vector = new Vector3();
        foreach (var item in currentKeys)
        {
            if (item.Value)
            {
                if (MoveRelativeToRotation)
                {
                    switch (item.Key)
                    {
                        case KeyCode.W:
                            vector += CopyTransform.up * movementForce;
                            break;
                        case KeyCode.A:
                            vector += -CopyTransform.right * movementForce;
                            break;
                        case KeyCode.S:
                            vector += -CopyTransform.up * movementForce;
                            break;
                        case KeyCode.D:
                            vector += CopyTransform.right * movementForce;
                            break;
                    }
                }
                else
                {
                    switch (item.Key)
                    {
                        case KeyCode.W:
                            vector += UpVector3 * movementForce;
                            break;
                        case KeyCode.A:
                            vector += -RightVector3 * movementForce;
                            break;
                        case KeyCode.S:
                            vector += -UpVector3 * movementForce;
                            break;
                        case KeyCode.D:
                            vector += RightVector3 * movementForce;
                            break;
                    }
                }

            }
        }
        return vector;
    }

    void UpdateInternalRotation()
    {
        _internalRotation = CopyTransform.localRotation;
    }
}
