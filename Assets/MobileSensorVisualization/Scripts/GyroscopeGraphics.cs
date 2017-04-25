/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;

namespace MobileSensor
{
    public class GyroscopeGraphics : MonoBehaviour
    {

        public Transform cube;

        // Use this for initialization
        void Start()
        {
            Input.gyro.enabled = true;
        }
	
        // Update is called once per frame
        void FixedUpdate()
        {
            cube.up = -Input.gyro.gravity;
        }

    }
}