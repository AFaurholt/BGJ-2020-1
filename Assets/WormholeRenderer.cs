using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WormholeRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter filter;
    [Space]
    [SerializeField, Range(4, 90)] private int resolution = 10;
    [SerializeField, Range(2, 10)] private int length = 5;
    [SerializeField, Range(0.2f, 5)] private float radius = 1;

    private void Update()
    {
        if(filter != null)
        {
            Mesh mesh = WormholeMeshGenerator.GetCylinder(resolution, length, radius);
            filter.mesh = mesh;
        }
    }
}
