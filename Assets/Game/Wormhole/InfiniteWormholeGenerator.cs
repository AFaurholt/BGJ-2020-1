using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormholeGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter filterTemplate = null;
    [SerializeField] DisplayedWormholeSettings settings = new DisplayedWormholeSettings();
    [Space]
    [SerializeField] private int meshCount = 5;
    [SerializeField] private float killDistance = -500;

    private int seed;
    private float noiseStartOffset;
    private int i;

    private Queue<MeshFilter> filters = new Queue<MeshFilter>();
    private Queue<CapsuleCollider> colliders = new Queue<CapsuleCollider>();

    List<RingTransform> rings = new List<RingTransform>();

    private void Awake()
    {
        i = 0;
        seed = UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        noiseStartOffset = UnityEngine.Random.Range(15f, 125f);

        for (int i = 0; i < meshCount; i++)
        {
            GenerateSegment(takeFromPool: false);
        }
    }

    private void Update()
    {
        if (filters.Peek().transform.localPosition.z < killDistance)
        {
            GenerateSegment();
        }
    }

    private void GenerateSegment(bool takeFromPool = true)
    {
        MeshFilter filter = takeFromPool
            ? filters.Dequeue()
            : Instantiate(filterTemplate, transform);

        WormholeSettings holeSettings = (WormholeSettings)settings;
        float noiseOffset = noiseStartOffset + holeSettings.NoiseSampleInterval * holeSettings.Length * i;

        Vector3 globalPos = rings.Count > 0
            ? rings[rings.Count - 1].GlobalPos
            : Vector3.zero;

        Quaternion rot = rings.Count > 0
            ? rings[rings.Count - 1].Rot
            : Quaternion.identity;

        filter.transform.position = globalPos;

        UnityEngine.Random.InitState(seed);
        WormholeResult wormholeResult = WormholeMeshGenerator.GetWormhole(holeSettings, noiseOffset, true, rot, filter.transform);

        filter.mesh = wormholeResult.Mesh;

        i++;
        filters.Enqueue(filter);

        for (int i = 0; i < wormholeResult.Rings.Count - 1; i++)
        {
            Transform colliderTransform = null;
            if (!takeFromPool)
            {
                CapsuleCollider collider = new GameObject().AddComponent<CapsuleCollider>();
                collider.radius = holeSettings.Radius;
                collider.height = holeSettings.RingDistanceMultiplier + collider.radius * 2;
                collider.center = Vector3.forward * holeSettings.RingDistanceMultiplier / 2;
                collider.direction = 2;

                colliderTransform = collider.transform;
                colliders.Enqueue(collider);
            }
            else
            {
                CapsuleCollider collider = colliders.Dequeue();
                colliders.Enqueue(collider);

                colliderTransform = collider.transform;
            }

            colliderTransform.parent = filter.transform;
            RingTransform ring = wormholeResult.Rings[i];
            colliderTransform.position = ring.GlobalPos;
            colliderTransform.rotation = ring.GlobalRot;
        }

        rings.AddRange(wormholeResult.Rings);
        rings.RemoveAt(rings.Count - holeSettings.Length);

        if (takeFromPool)
            rings.RemoveRange(0, holeSettings.Length - 1);
    }

    public RingTransform FindNext(float distanceFromZero)
    {
        return RingUtils.FindNext(transform.forward * distanceFromZero, transform.forward, rings);
    }
}
