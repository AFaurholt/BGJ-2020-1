﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GibbingScript : MonoBehaviour
{

    //public LayerMask mask;
    public Animator anim;

    private void Start()
    {
        //Gib();
    }

    private void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Gib();
        Debug.Log("hello");
    }
    private void OnTriggerEnter(Collider other)
    {
        Gib();
        Debug.Log("hello");
    }

    void Gib()
    {
        Vector3 v = transform.position;
        gameObject.transform.parent = null;
        anim.Rebind();
        transform.position = v;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        gameObject.AddComponent<StaticRelativeObject>();
        this.GetComponent<Collider>().enabled = false;
        NoiseTransform nt = this.GetComponent<NoiseTransform>();
        if(nt != null)
        {
            nt.enabled = false;
        }
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb. AddForce(new Vector3(Random.Range(-10,10), Random.Range(-10, 10), Random.Range(-10, 10)), ForceMode.Impulse);
    }
}
