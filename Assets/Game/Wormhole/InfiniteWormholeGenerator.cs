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

    private Transform lastTransform = null;
    private WormholeResult lastWormholeResult = new WormholeResult(null, Vector3.zero, Quaternion.identity);

    private int seed;
    private float noiseStartOffset;
    private int i;

    private Queue<MeshFilter> filters = new Queue<MeshFilter>();

    private void Awake()
    {
        i = 0;
        seed = Random.Range(int.MinValue, int.MaxValue);
        noiseStartOffset = Random.Range(15f, 125f);

        lastTransform = transform;
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

        filter.transform.localPosition = lastTransform.TransformPoint(lastWormholeResult.LastPos);

        Random.InitState(seed);
        lastWormholeResult = WormholeMeshGenerator.GetWormhole(holeSettings, noiseOffset, true, lastWormholeResult.LastRot);
        filter.mesh = lastWormholeResult.Mesh;

        lastTransform = filter.transform;
        i++;
        filters.Enqueue(filter);
    }
}
