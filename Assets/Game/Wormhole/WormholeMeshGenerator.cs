using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal struct WormholeResult
{
    public Mesh Mesh;
    public Vector3 LastPos;
    public Quaternion LastRot;

    public WormholeResult(Mesh mesh, Vector3 lastPos, Quaternion lastRot)
    {
        Mesh = mesh;
        LastPos = lastPos;
        LastRot = lastRot;
    }
}

public struct WormholeSettings
{
    public enum RotationMode
    {
        AngleFromAxis, AnglePerStep
    }

    public readonly Vector3 Up;
    public readonly Vector3 Forward;
    public readonly Vector3 Origin;
    public readonly RotationMode Mode;
    public readonly int Resolution;
    public readonly int Length;
    public readonly float Radius;
    public readonly float RotationMax;
    public readonly float NoiseSampleInterval;
    public readonly float RingDistanceMultiplier;

    public WormholeSettings(Vector3 up, Vector3 forward, Vector3 origin, RotationMode mode, int resolution, int length, float radius, float rotationMax, float noiseSampleInterval, float ringDistanceMultiplier)
    {
        Up = up.normalized;
        Forward = forward.normalized;
        Origin = origin;
        Mode = mode;
        Resolution = resolution;
        Length = length;
        Radius = radius;
        RotationMax = rotationMax;
        NoiseSampleInterval = noiseSampleInterval;
        RingDistanceMultiplier = ringDistanceMultiplier;
    }
}

public static class WormholeMeshGenerator
{
    private const int trisPerSegment = 4;

    internal static WormholeResult GetWormhole(WormholeSettings settings, float noiseOffset, bool startFlat, Quaternion startRotation)
    {
        Vector3[] verts = new Vector3[settings.Resolution * settings.Length];
        Vector3[] normals = new Vector3[verts.Length];

        (Vector3 T, Quaternion R) lastRingRT = (Vector3.zero, Quaternion.identity);

        switch (settings.Mode)
        {
            case WormholeSettings.RotationMode.AnglePerStep:
                lastRingRT = GenerateRingsRelativeToLast(settings, verts, normals, noiseOffset);
                break;
            case WormholeSettings.RotationMode.AngleFromAxis:
                lastRingRT = GenerateRingsRelativeToAxis(settings, verts, normals, startFlat, noiseOffset, startRotation);
                break;
            default:
                break;
        }

        int[] tris = BridgeLoops(settings);

        Mesh mesh = new Mesh()
        {
            vertices = verts,
            triangles = tris,
            normals = normals
        };

        return new WormholeResult(mesh, lastRingRT.T, lastRingRT.R);
    }

    private static int[] BridgeLoops(WormholeSettings settings)
    {
        int[] tris = new int[(settings.Resolution * (settings.Length - 1)) * trisPerSegment * 3];
        for (int ring = 0; ring < settings.Length - 1; ring++)
        {

            for (int vert = 0; vert < settings.Resolution; vert++)
            {
                // Generating most of the ring
                int vertPos = ring * settings.Resolution + vert;
                int trisPos = vertPos * trisPerSegment * 3;

                tris[trisPos + 0] = GetIndexOnCylinder(vertPos, 0, 0, settings.Resolution);
                tris[trisPos + 1] = GetIndexOnCylinder(vertPos, 1, 1, settings.Resolution);
                tris[trisPos + 2] = GetIndexOnCylinder(vertPos, 1, 0, settings.Resolution);

                tris[trisPos + 3] = GetIndexOnCylinder(vertPos, 0, 0, settings.Resolution);
                tris[trisPos + 4] = GetIndexOnCylinder(vertPos, 0, 1, settings.Resolution);
                tris[trisPos + 5] = GetIndexOnCylinder(vertPos, 1, 1, settings.Resolution);

                tris[trisPos + 6] = GetIndexOnCylinder(vertPos, 0, 0, settings.Resolution);
                tris[trisPos + 7] = GetIndexOnCylinder(vertPos, 1, 0, settings.Resolution);
                tris[trisPos + 8] = GetIndexOnCylinder(vertPos, 1, 1, settings.Resolution);

                tris[trisPos + 9] = GetIndexOnCylinder(vertPos, 0, 0, settings.Resolution);
                tris[trisPos + 10] = GetIndexOnCylinder(vertPos, 1, 1, settings.Resolution);
                tris[trisPos + 11] = GetIndexOnCylinder(vertPos, 0, 1, settings.Resolution);
            }
        }

        return tris;
    }

    private static (Vector3 position, Quaternion finalRotation) GenerateRingsRelativeToAxis(WormholeSettings settings, Vector3[] verts, Vector3[] normals, bool startFlat, float noiseOffset, Quaternion startRotation)
    {
        float noisePosition = noiseOffset;
        float xNoiseSeed = Random.Range(-1236f, 21756f);
        float yNoiseSeed = Random.Range(-8775f, 63287f) + xNoiseSeed;
        float zNoiseSeed = Random.Range(-9849f, 153f) + yNoiseSeed;

        Vector3 currentPosition = settings.Origin;
        for (int i = 0; i < settings.Length; i++)
        {

            Quaternion randomQuaternion = Quaternion.Euler(
                (Mathf.PerlinNoise(noisePosition, xNoiseSeed) * 2 - 1) * settings.RotationMax,
                (Mathf.PerlinNoise(noisePosition, yNoiseSeed) * 2 - 1) * settings.RotationMax,
                (Mathf.PerlinNoise(noisePosition, zNoiseSeed) * 2 - 1) * settings.RotationMax
            );

            if(startFlat)
                randomQuaternion = Quaternion.Slerp(startRotation, randomQuaternion, i / 3f);

            noisePosition += settings.NoiseSampleInterval;

            Vector3 randomForward = randomQuaternion * settings.Forward;
            Vector3 randomUp = randomQuaternion * settings.Up;

            FillCircle(settings, currentPosition, randomUp, randomForward, verts, normals, i);

            if (i != settings.Length - 1)
                currentPosition += randomForward * settings.RingDistanceMultiplier;
            else
                return (currentPosition, randomQuaternion);
        }

        return (Vector3.zero, Quaternion.identity);
    }

    private static (Vector3 position, Quaternion finalRotation) GenerateRingsRelativeToLast(WormholeSettings settings, Vector3[] verts, Vector3[] normals, float noiseOffset)
    {
        float noisePosition = noiseOffset;
        float xNoiseSeed = Random.Range(-1236f, 21756f);
        float yNoiseSeed = Random.Range(-8775f, 63287f) + xNoiseSeed;
        float zNoiseSeed = Random.Range(-9849f, 153f) + yNoiseSeed;

        Vector3 currentPosition = settings.Origin;
        Vector3 currentForward = settings.Forward;
        Vector3 currentUp = settings.Up;
        for (int i = 0; i < settings.Length; i++)
        {
            Quaternion randomQuaternion = Quaternion.Euler(
                (Mathf.PerlinNoise(noisePosition, xNoiseSeed) * 2 - 1) * settings.RotationMax,
                (Mathf.PerlinNoise(noisePosition, yNoiseSeed) * 2 - 1) * settings.RotationMax,
                (Mathf.PerlinNoise(noisePosition, zNoiseSeed) * 2 - 1) * settings.RotationMax
            );

            noisePosition += settings.NoiseSampleInterval;

            currentForward = randomQuaternion * currentForward;
            currentUp = randomQuaternion * currentUp;

            FillCircle(settings, currentPosition, currentUp, currentForward, verts, normals, i);

            if (i != settings.Length - 1)
                currentPosition += currentForward * settings.RingDistanceMultiplier;
        }

        return (currentPosition, Quaternion.FromToRotation(settings.Forward, currentForward));
    }

    private static void FillCircle(WormholeSettings settings, Vector3 position, Vector3 up, Vector3 forward, Vector3[] verts, Vector3[] normals, int ringID)
    {
        Quaternion rotationPerStep = Quaternion.AngleAxis(360f / settings.Resolution, forward);
        int startingIndex = ringID * settings.Resolution;

        for (int i = 0; i < settings.Resolution; i++)
        {
            int currentIndex = startingIndex + i;

            verts[currentIndex] = position + up * settings.Radius;
            normals[currentIndex] = -up;

            up = rotationPerStep * up;
        }
    }

    private static int GetIndexOnCylinder(int pos, int segment, int ring, int resolution)
    {
        // Gets ring offset, aka, index of first vert om ring
        int ringOffset = (pos / resolution) * resolution;
        
        // Add ands round position in ring space
        int posOnRing = pos - ringOffset;
        posOnRing += segment;
        posOnRing %= resolution;

        // Ands removed offset
        pos = posOnRing + ringOffset;

        // Returns position offsetted by ring count
        return pos + ring * resolution;
    }
}