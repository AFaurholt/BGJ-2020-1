using UnityEngine;

[System.Serializable]
public class DisplayedWormholeSettings
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

    public static explicit operator WormholeSettings(DisplayedWormholeSettings s)
        => new WormholeSettings(Vector3.up, Vector3.forward, Vector3.zero, s.rotationMode, s.vertsPerRing, s.ringAmount, s.ringRadius, s.maxRandomAngle, s.noiseVariation, s.ringLength);
}
