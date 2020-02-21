using System;
using System.Collections.Generic;
using UnityEngine;

public static class RingUtils
{
    /// <param name="sortedRings">Ring should be sorted from first to last</param>
    public static RingTransform FindNext(Vector3 searchPoint, Vector3 direction, List<RingTransform> sortedRings)
    {
        for (int i = 0; i < sortedRings.Count; i++)
        {
            var diff = sortedRings[i].GlobalPos - searchPoint;
            if(Vector3.Dot(direction, diff) > 0)
            {
                return sortedRings[i];
            }
        }
        return sortedRings[sortedRings.Count - 1];
    }
}