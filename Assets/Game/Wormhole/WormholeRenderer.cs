using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WormholeRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter filter = null;
    [Space]
    [SerializeField] DisplayedSettings settings = new DisplayedSettings();
    [Space]
    [SerializeField] private int seed = 28362;

    [System.Serializable]
    public class DisplayedSettings
    {
        [Header("Ring")]
        [SerializeField, Range(4, 70)] private int vertsPerRing = 10;
        [SerializeField, Range(2, 5000)] private int ringAmount = 5;
        [SerializeField, Range(1, 500)] private float ringLength = 20;
        [SerializeField, Range(0.2f, 50)] private float ringRadius = 1;
        [Header("Rotation")]
        [SerializeField] private WormholeSettings.RotationMode rotationMode = WormholeSettings.RotationMode.AngleFromAxis;
        [SerializeField, Range(0, 180f)] private float maxRandomAngle = 2;
        [SerializeField, Range(0.001f, 2f)] private float noiseVariation = 0.2f;
        [Header("Axes")]
        [SerializeField] private Vector3 up = Vector3.up;
        [SerializeField] private Vector3 forward = Vector3.forward;
        [SerializeField] private Vector3 offset = Vector3.zero;

        public static explicit operator WormholeSettings(DisplayedSettings s)
        {
            return new WormholeSettings(s.up, s.forward, s.offset, s.rotationMode, s.vertsPerRing, s.ringAmount, s.ringRadius, s.maxRandomAngle, s.noiseVariation, s.ringLength);
        }
    }

    private void Update()
    {
        if(filter != null)
        {
            Random.InitState(seed);
            Mesh mesh = WormholeMeshGenerator.GetWormhole((WormholeSettings)settings);
            filter.mesh = mesh;
        }
    }
}
