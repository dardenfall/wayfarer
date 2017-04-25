/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;

namespace MobileSensor
{
    public class CompassGraphics : MonoBehaviour
    {

        public Transform cube;

        // Use this for initialization
        void Start()
        {
            Input.compass.enabled = true;
        }
	
        // Update is called once per frame
        void FixedUpdate()
        {
            cube.rotation = Quaternion.Euler(0, 0, Input.compass.trueHeading);
        }

    }
}