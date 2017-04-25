/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace InfiniteRunVR
{
    public class TrackManager : MonoBehaviour
    {

        public GameObject prefab;

        private List<Track> _tracks = new List<Track>();
        private float _viewingDistance;
        private int _trackNum;
        private Vector3 _orgDir;
        private PlayerController _player;

        // Use this for initialization
        void Start()
        {
        }
	
        // Update is called once per frame
        void Update()
        {
	
        }
        #region Straight Track 
        public void Init(PlayerController target, float viewingDistance, Vector3 dir)
        {
            // init pretrack
            _orgDir = dir;
            transform.position = setAxisBasedDirection(transform.position, -viewingDistance);
            _viewingDistance = viewingDistance;
            _player = target;
        }

        public void InstantitateNewTrack()
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.transform.SetParent(transform);

            Track newTrack = go.GetComponent<Track>();
            newTrack.transform.localEulerAngles = Vector3.zero;
            if (_tracks.Count > 0)
            {
                newTrack.transform.position = _tracks[_tracks.Count - 1].end.position;
            }
            else
            {
                newTrack.transform.localPosition = Vector3.zero;
            }

            _trackNum++;
            go.name = "Track" + _trackNum;

            _tracks.Add(newTrack);

        }

        public void DestroyUnSeenTrack(Vector3 playerPosition)
        {
            if (_tracks.Count > 0)
            {
                if (getAxisBasedDirection(_tracks[0].end.position) < getAxisBasedDirection(playerPosition) - _viewingDistance)
                {
                    GameObject go = _tracks[0].gameObject;
                    _tracks.Remove(_tracks[0]);
                    Destroy(go);
                }
            }
        }

        public bool IsTrackInView(Vector3 skylinePosition)
        {
            if (_tracks.Count > 0)
            {
                return getAxisBasedDirection(skylinePosition) < getAxisBasedDirection(_tracks[_tracks.Count - 1].end.position);
            }
            return false;
        }

        float getAxisBasedDirection(Vector3 position)
        {
            if (_orgDir == Vector3.forward)
            {
                return position.z;
            } else if (_orgDir == Vector3.right)
            {
                return position.x;
            } else if (_orgDir == Vector3.left)
            {
                return -position.x;
            } else if (_orgDir == Vector3.back)
            {
                return -position.z;
            }
            return position.z;
        }

        Vector3 setAxisBasedDirection(Vector3 position, float axis)
        {
            if (_orgDir == Vector3.forward)
            {
                return new Vector3(position.x, position.y, axis);
            } else if (_orgDir == Vector3.right)
            {
                return new Vector3(axis, position.y, position.z);
            } else if (_orgDir == Vector3.left)
            {
                return new Vector3(-axis, position.y, position.z);
            } else if (_orgDir == Vector3.back)
            {
                return new Vector3(position.x, position.y, -axis);
            }
            return new Vector3(position.x, position.y, axis);
        }
        #endregion

   
    }
}