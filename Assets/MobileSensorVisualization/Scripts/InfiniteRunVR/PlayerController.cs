/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using MobileSensor;

namespace InfiniteRunVR
{
    public class PlayerController : MonoBehaviour
    {

        [Tooltip("Speed unit km/h")]
        public float Speed;
        protected float _speed;

        [Tooltip("Max distance perspective unit m")]
        public float FieldOfViewDistance;

        protected TrackManager _trackManager;
       

        // Use this for initialization
        protected void Start()
        {
            // resgiter trackManager event 
            if (_trackManager == null)
            {
                _trackManager = (TrackManager)FindObjectOfType(typeof(TrackManager));
            }
            _trackManager.Init(this, FieldOfViewDistance, transform.root.forward);

            // resgiter step counter event 
            Input.gyro.enabled = true;
            _speed = Speed;
          
        }
	 
        // Update is called once per frame
        protected void FixedUpdate()
        {
            updateMovement();

            // update speed
            _speed = Speed;
        }

        float convertFromKMToM(float velocity)
        {
            return velocity * 1000 / 3600f;
        }

        void updateMovement()
        {
            transform.localPosition += new Vector3(0, 0, convertFromKMToM(Time.fixedDeltaTime * Speed));
            if (!_trackManager.IsTrackInView(transform.position + transform.root.forward * FieldOfViewDistance))
            {
                _trackManager.InstantitateNewTrack();
            }
            _trackManager.DestroyUnSeenTrack(transform.position);
        }

       
    }
}