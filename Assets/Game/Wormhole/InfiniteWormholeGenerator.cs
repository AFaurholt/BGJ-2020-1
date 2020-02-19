using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteWormholeGenerator : MonoBehaviour
{
    [SerializeField] private MeshFilter filterTemplate = null;
    [SerializeField] DisplayedWormholeSettings settings = new DisplayedWormholeSettings();

    private Transform lastTransform = null;
    private WormholeResult lastWormholeResult = new WormholeResult(null, Vector3.zero, Quaternion.identity);

    private int seed;
    private float noiseStartOffset;
    private int i;

    private void Awake()
    {
        i = 0;
        seed = Random.Range(124, 128734714);
        noiseStartOffset = Random.Range(15f, 125f);

        lastTransform = transform;
        GenerateSegment();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            GenerateSegment();
    }

    private void GenerateSegment()
    {
        MeshFilter filter = Instantiate(filterTemplate, transform);

        WormholeSettings holeSettings = (WormholeSettings)settings;
        float noiseOffset = noiseStartOffset + holeSettings.NoiseSampleInterval * holeSettings.Length * i;

        filter.transform.localPosition = lastTransform.TransformPoint(lastWormholeResult.LastPos);

        Random.InitState(seed);
        lastWormholeResult = WormholeMeshGenerator.GetWormhole(holeSettings, noiseOffset, true, lastWormholeResult.LastRot);
        filter.mesh = lastWormholeResult.Mesh;

        lastTransform = filter.transform;
        i++;
    }
}
