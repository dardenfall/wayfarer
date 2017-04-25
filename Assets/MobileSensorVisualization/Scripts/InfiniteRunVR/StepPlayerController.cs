/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using MobileSensor;

namespace InfiniteRunVR
{
    [RequireComponent(typeof(StepCounter))]
    public class StepPlayerController : PlayerController
    {
        protected StepCounter _stepCounter;

        // Use this for initialization
        protected void Start()
        {
            base.Start();

            // resgiter step counter event 
            Input.gyro.enabled = true;
            _speed = Speed;
            if (_stepCounter == null)
            {
                _stepCounter = (StepCounter)FindObjectOfType(typeof(StepCounter));
            }
            _stepCounter.speedCalled = (float s)=>{ Speed = s;};
        }
	 
        // Update is called once per frame
        protected void FixedUpdate()
        {
            base.FixedUpdate();
        }
       
    }
}