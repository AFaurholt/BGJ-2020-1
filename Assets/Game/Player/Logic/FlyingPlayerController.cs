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

    private Rigidbody _rb;

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
    }
    private void Update()
    {
        //movement
        _currentKeys = InputUtil.GetIfKeysHeld(_controls);
    }
    private void FixedUpdate()
    {
        Vector3 currentMoveVector = GetMovementVector(_currentKeys, MoveForce);


        _rb.velocity = GravityVector3 + currentMoveVector;
    }

    /// <summary>
    /// Gets the movement vector3 based on current keys. Only supports WASD controls
    /// </summary>
    Vector3 GetMovementVector(IDictionary<KeyCode, bool> currentKeys, float movementForce)
    {
        Vector3 vector = new Vector3();
        foreach (var item in currentKeys)
        {
            if (item.Value)
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
        return vector;
    }
}
