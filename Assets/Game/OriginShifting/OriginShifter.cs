using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OriginShifter
{
    public static event Action<Vector3> Shifted;

    public static void MoveOriginBy(Vector3 delta)
    {
        Shifted?.Invoke(-delta);
    }
}
