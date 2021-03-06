﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingShifter : MonoBehaviour
{
    [SerializeField] private float speed = 2f;

    void Update()
    {
        OriginShifter.MoveOriginBy(speed * transform.forward * Time.deltaTime);       
    }
}
