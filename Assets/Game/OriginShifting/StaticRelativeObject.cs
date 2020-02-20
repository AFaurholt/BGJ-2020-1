using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticRelativeObject : MonoBehaviour
{
    private void OnEnable()
    {
        OriginShifter.Shifted += OnShift;
    }

    private void OnDisable()
    {
        OriginShifter.Shifted -= OnShift;
    }

    private void OnShift(Vector3 delta)
    {
        transform.position += delta;
    }
}
