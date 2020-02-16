using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHoleProto : MonoBehaviour
{
    public GameObject holePrefab;
    GameObject tempHole;
    public float maxSize = 1;
    float size = 0;

    public float timeToMaxSize = 2;
    float time = 0;

    bool isCharging = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            if (!isCharging)
            {
                tempHole = Instantiate(holePrefab,this.transform);
                tempHole.transform.localScale = new Vector3(0, 0, 0);
                time = 0;
                size = 0;
                isCharging = true;
            }
            else
            {
                time += Time.deltaTime;
                size = Mathf.Min(time / timeToMaxSize * maxSize, maxSize);
                Debug.Log(size);
                tempHole.transform.localScale = new Vector3(size,size,size);
            }
        }
        else
        {
            if (isCharging)
            {
                tempHole.transform.parent = null;
                tempHole = null;
                isCharging = false;
            }
        }
    }

    
}
