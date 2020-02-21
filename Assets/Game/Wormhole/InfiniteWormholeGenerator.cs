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

        rings.AddRange(wormholeResult.Rings);
        rings.RemoveAt(rings.Count - holeSettings.Length);
        if (takeFromPool)
            rings.RemoveRange(0, holeSettings.Length - 1);
    }
}
