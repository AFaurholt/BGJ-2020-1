using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WormholeRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter[] filters = null;
    [Space]
    [SerializeField] DisplayedSettings settings = new DisplayedSettings();
    [Space]
    [SerializeField] private int seed = 28362;
    [SerializeField] private float noiseStartOffset = 21f;

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

        public static explicit operator WormholeSettings(DisplayedSettings s)
        {
            return new WormholeSettings(Vector3.up, Vector3.forward, Vector3.zero, s.rotationMode, s.vertsPerRing, s.ringAmount, s.ringRadius, s.maxRandomAngle, s.noiseVariation, s.ringLength);
        }
    }

    private void Update()
    {
        if (filters != null)
        {
            Random.InitState(seed);
            WormholeSettings holeSettings = (WormholeSettings)settings;

            WormholeResult lastWormhole = new WormholeResult();
            Transform lastTransform = transform;

            for (int i = 0; i < filters.Length; i++)
            {
                float noiseOffset = noiseStartOffset + holeSettings.NoiseSampleInterval * holeSettings.Length * i;

                filters[i].transform.localPosition = lastTransform.TransformPoint(lastWormhole.LastPos);

                Random.InitState(seed);
                lastWormhole = WormholeMeshGenerator.GetWormhole(holeSettings, noiseOffset, true, lastWormhole.LastRot);
                filters[i].mesh = lastWormhole.Mesh;

                lastTransform = filters[i].transform;
            }
        }
    }
}
