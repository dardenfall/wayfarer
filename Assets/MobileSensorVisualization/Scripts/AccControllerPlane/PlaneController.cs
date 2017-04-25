/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using MobileSensor;
using InfiniteRunVR;

namespace AccControlledPlane
{
    public class PlaneController : PlayerController
    {
        [Tooltip("Horizontal range when the plane turns left/right")]
        public Vector2 rangeOfHozrizon;

        [Tooltip("The pivot of the 3D mesh")]
        public Transform plane;

        [Tooltip("The speed of the plane when turn left/right")]
        public float speedOfTurn;

        [Tooltip("The speed of the plane when speed up")]
        public float speedOfSpeedUp;

        float triggerToLean = 0.1f;


        // Use this for initialization
        void Start()
        {
            base.Start();
           
        }
	 
        // Update is called once per frame
        void FixedUpdate()
        {
            // move forward 
            base.FixedUpdate();

            // change direction based gyro 
            lean();

            // move local left/right
            moveLeftRight();

            // change speed based on gyro 
            speedUp();
        }

        void lean(){
            Vector3 up = -Input.gyro.gravity;
            up = new Vector3(-up.x, up.y, up.z);
            plane.up = up;
        }
       
        void moveLeftRight(){
            // calculate angle 
            float right = 0;
            if (plane.right.y > triggerToLean)
            { // turn left 
                right = -1;
            }
            else if (plane.right.y < -triggerToLean)
            { // turn right 
                right = 1;
            }

            // stop when reach border 
            float deltaX = right * speedOfTurn * Time.fixedDeltaTime;
            float localX = Mathf.Clamp(plane.position.x + deltaX, rangeOfHozrizon.x, rangeOfHozrizon.y);

            // move 
            plane.position = new Vector3(localX,plane.position.y,plane.position.z);
        }
       
        void speedUp(){
            float dir = 0;
            if (plane.forward.y > triggerToLean)
            { // hold back 
                dir = -1;
            }
            else if (plane.forward.y < -triggerToLean)
            { // speed up 
                dir = 1;
            }
            float deltaBasedGyro = Mathf.Abs(plane.forward.y*1f / triggerToLean);
            Speed += dir * speedOfSpeedUp * Time.fixedDeltaTime * deltaBasedGyro;
            Speed = Mathf.Clamp(Speed, 0.2f, 30);
    
        }

    }
}