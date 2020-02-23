using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class WormholeRenderer : MonoBehaviour
{
    [SerializeField] private MeshFilter[] filters = null;
    [Space]
    [SerializeField] DisplayedWormholeSettings settings = new DisplayedWormholeSettings();
    [Space]
    [SerializeField] private int seed = 28362;
    [SerializeField] private float noiseStartOffset = 21f;

    private void Update()
    {
        if (filters != null)
        {
            Random.InitState(seed);
            WormholeSettings holeSettings = (WormholeSettings)settings;

            RingTransform lastRing = new RingTransform();
            Transform lastTransform = transform;

            for (int i = 0; i < filters.Length; i++)
            {
                float noiseOffset = noiseStartOffset + holeSettings.NoiseSampleInterval * holeSettings.Length * i;

                filters[i].transform.localPosition = lastTransform.TransformPoint(lastRing.Pos);

                Random.InitState(seed);
                WormholeResult wormhole = WormholeMeshGenerator.GetWormhole(holeSettings, noiseOffset, true, lastRing.Rot, filters[i].transform);
                filters[i].mesh = wormhole.Mesh;
                lastRing = wormhole.Rings[wormhole.Rings.Count - 1];

                lastTransform = filters[i].transform;
            }
        }
    }
}
