using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WormholeExitDetection : MonoBehaviour
{
    [SerializeField] private string wormholeTag = "Wormhole";
    [SerializeField] private UnityEvent exitedWormhole = new UnityEvent();
    private int counter = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(wormholeTag))
        {
            counter++;
        }
        if(other.gameObject.layer == 10)
        {
            if (exitedWormhole != null)
                exitedWormhole.Invoke();
            GetComponent<GibbingScript>().Gib();
            Debug.Log("Hello my dudes");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(wormholeTag))
        {
            counter--;
            if (counter == 0)
            {
                if(exitedWormhole != null)
                    exitedWormhole.Invoke();
                GetComponent<GibbingScript>().Gib();
            }
        }
        Debug.Log(other.tag);
    }
}
