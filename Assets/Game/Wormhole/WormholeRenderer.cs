using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WormholeRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter filter = null;
    [Space]
    [SerializeField, Range(4, 90)] private int resolution = 10;
    [SerializeField, Range(2, 900)] private int length = 5;
    [SerializeField, Range(0.2f, 5)] private float radius = 1;
    [Space]
    [SerializeField, Range(0, 90f)] private float randomRotation = 2;
    [SerializeField, Range(0.05f, 2f)] private float noiseSampleInterval = 0.2f;
    [SerializeField] private WormholeMeshGenerator.RotationMode rotationMode = WormholeMeshGenerator.RotationMode.AngleFromAxis;
    [SerializeField] private int seed = 28362;

    private void Update()
    {
        if(filter != null)
        {
            Random.InitState(seed);
            Mesh mesh = WormholeMeshGenerator.GetCylinder(transform.up, transform.forward, Vector2.zero, rotationMode, resolution, length, radius, randomRotation, noiseSampleInterval);
            filter.mesh = mesh;
        }
    }
}
