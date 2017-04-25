/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace MobileSensor
{
    public class SensorManager : MonoBehaviour
    {

        public enum SensorType
        {
            Accelerometer,
            Gyroscope,
            Compass
        }

        public Toggle accelerometerToggle;
        public Toggle gyroscopeToggle;
        public Toggle compassToggle;

        public SensorGraphics accelerometerGraph;
        public SensorGraphics gyroscopeGraph;
        public SensorGraphics compassGraph;


        // Use this for initialization
        void Start()
        {
            accelerometerGraph.SetGraphicSize(new Vector2(10, 3), new Vector2(Screen.width, 500));
            accelerometerGraph.Init();
            gyroscopeGraph.SetGraphicSize(new Vector2(10, 3), new Vector2(Screen.width, 500));
            gyroscopeGraph.Init();
            compassGraph.SetGraphicSize(new Vector2(10, 60), new Vector2(Screen.width, 500));
            compassGraph.Init();

        }
	
        // Update is called once per frame
        void Update()
        {
            Input.gyro.enabled = gyroscopeToggle.isOn;
            Input.compass.enabled = compassToggle.isOn;

            accelerometerGraph.EnableDisplay(accelerometerToggle.isOn);
            gyroscopeGraph.EnableDisplay(gyroscopeToggle.isOn);
            compassGraph.EnableDisplay(compassToggle.isOn);

        }

    }
}