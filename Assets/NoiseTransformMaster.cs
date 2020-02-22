using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseTransformMaster : MonoBehaviour
{
    public List<NoiseTransform> noiseScripts;

    public bool startEnabled = false;

    void Start()
    {
        if (startEnabled)
            StartNoise();
        else
            EndNoise();
    }

    public void StartNoise()
    {
        foreach ( NoiseTransform nt in noiseScripts)
        {
            nt.enabled = true;
        }
    }

    public void EndNoise()
    {
        foreach (NoiseTransform nt in noiseScripts)
        {
            nt.enabled = false;
        }
    }
}
