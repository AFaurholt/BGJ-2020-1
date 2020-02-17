using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WormholeRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter filter;
    [Space]
    [SerializeField, Range(4, 90)] private int resolution = 10;
    [SerializeField, Range(2, 90)] private int length = 5;
    [SerializeField, Range(0.2f, 5)] private float radius = 1;
    [Space]
    [SerializeField, Range(0, 10f)] private float randomRotation = 2;
    [SerializeField] private int seed;

    private void Update()
    {
        if(filter != null)
        {
            Random.InitState(seed);
            Mesh mesh = WormholeMeshGenerator.GetCylinder(resolution, length, radius, randomRotation);
            filter.mesh = mesh;
        }
    }
}
