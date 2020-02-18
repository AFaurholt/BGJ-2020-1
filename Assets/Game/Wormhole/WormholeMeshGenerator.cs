using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WormholeMeshGenerator
{
    public enum RotationMode
    {
        AngleFromAxis, AnglePerStep
    }

    private const int trisPerSegment = 4;

    internal static Mesh GetCylinder(Vector3 up, Vector3 forward, Vector3 position, RotationMode mode, int resolution, int length, float radius, float rotationMax, float noiseSampleInterval, float ringDistanceMultiplier)
    {
        Vector3[] verts = new Vector3[resolution * length];
        Vector3[] normals = new Vector3[verts.Length];

        switch (mode)
        {
            case RotationMode.AnglePerStep:
                GenerateRingsRelativeToLast(up, forward, position, resolution, length, radius, rotationMax, noiseSampleInterval, ringDistanceMultiplier, verts, normals);
                break;
            case RotationMode.AngleFromAxis:
                float noisePosition = 0;
                float xNoiseSeed = Random.Range(-1236f, 21756f);
                float yNoiseSeed = Random.Range(-8775f, 63287f) + xNoiseSeed;

                Vector3 currentPosition = position;
                for (int i = 0; i < length; i++)
                {
                    Quaternion randomQuaternion = Quaternion.Euler(
                        (Mathf.PerlinNoise(noisePosition, xNoiseSeed) * 2 - 1) * rotationMax,
                        (Mathf.PerlinNoise(noisePosition, yNoiseSeed) * 2 - 1) * rotationMax,
                        0);

                    noisePosition += noiseSampleInterval;

                    Vector3 randomForward = randomQuaternion * forward;
                    Vector3 randomUp = randomQuaternion * up;

                    FillCircle(currentPosition, randomUp, randomForward, radius, resolution, verts, normals, i * resolution);
                    currentPosition += randomForward * ringDistanceMultiplier;
                }
                break;
            default:
                break;
        }

        int[] tris = new int[(resolution * (length - 1)) * trisPerSegment * 3];
        for (int ring = 0; ring < length - 1; ring++)
        {

            for (int vert = 0; vert < resolution; vert++)
            {
                // Generating most of the ring
                int vertPos = ring * resolution + vert;
                int trisPos = vertPos * trisPerSegment * 3;

                tris[trisPos + 0] = GetIndexOnCylinder(vertPos, 0, 0, resolution);
                tris[trisPos + 1] = GetIndexOnCylinder(vertPos, 1, 1, resolution);
                tris[trisPos + 2] = GetIndexOnCylinder(vertPos, 1, 0, resolution);

                tris[trisPos + 3] = GetIndexOnCylinder(vertPos, 0, 0, resolution);
                tris[trisPos + 4] = GetIndexOnCylinder(vertPos, 0, 1, resolution);
                tris[trisPos + 5] = GetIndexOnCylinder(vertPos, 1, 1, resolution);

                tris[trisPos + 6] = GetIndexOnCylinder(vertPos, 0, 0, resolution);
                tris[trisPos + 7] = GetIndexOnCylinder(vertPos, 1, 0, resolution);
                tris[trisPos + 8] = GetIndexOnCylinder(vertPos, 1, 1, resolution);

                tris[trisPos + 9] = GetIndexOnCylinder(vertPos, 0, 0, resolution);
                tris[trisPos + 10] = GetIndexOnCylinder(vertPos, 1, 1, resolution);
                tris[trisPos + 11] = GetIndexOnCylinder(vertPos, 0, 1, resolution);
            }
        }

        return new Mesh()
        {
            vertices = verts,
            triangles = tris,
            normals = normals
        };
    }

    private static void GenerateRingsRelativeToLast(Vector3 up, Vector3 forward, Vector3 position, int resolution, int length, float radius, float rotationMax, float noiseSampleInterval, float ringDistanceMultiplier, Vector3[] verts, Vector3[] normals)
    {
        float noisePosition = 0;
        float xNoiseSeed = Random.value;
        float yNoiseSeed = Random.value + xNoiseSeed;

        Vector3 currentPosition = position;
        Vector3 currentForward = forward;
        Vector3 currentUp = up;
        for (int i = 0; i < length; i++)
        {
            Quaternion randomQuaternion = Quaternion.Euler(
                (Mathf.PerlinNoise(noisePosition, xNoiseSeed) * 2 - 1) * rotationMax,
                (Mathf.PerlinNoise(noisePosition, yNoiseSeed) * 2 - 1) * rotationMax,
                0);

            noisePosition += noiseSampleInterval;

            currentForward = randomQuaternion * currentForward;
            currentUp = randomQuaternion * currentUp;

            FillCircle(currentPosition, currentUp, currentForward, radius, resolution, verts, normals, i * resolution);
            currentPosition += currentForward * ringDistanceMultiplier;

        }
    }

    private static void FillCircle(Vector3 position, Vector3 up, Vector3 forward, float radius, int resolution, Vector3[] verts, Vector3[] normals, int startingIndex)
    {
        forward.Normalize();
        up.Normalize();

        Quaternion rotationPerStep = Quaternion.AngleAxis(360f / resolution, forward);

        for (int i = 0; i < resolution; i++)
        {
            int currentIndex = startingIndex + i;

            verts[currentIndex] = position + up * radius;
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
