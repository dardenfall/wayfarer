using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

namespace MobileSensor
{
    public class StepCounter : MonoBehaviour
    {
        public SensorGraphics accelerometer;

        public Action<int> stepCalled;
        public Action<float> speedCalled;

        public Slider sliderInterval;
        public Slider sliderThresholdHigh;
        public Slider sliderThresholdLow;

        public Text sliderTextInterval;
        public Text sliderTextThresholdHigh;
        public Text sliderTextThresholdLow;
        public Text stepText;
        public Text speedText;


        // alsway compare with X axis
        float _threshold = -1;


        KeyValuePair<float, float> _minValue;
        KeyValuePair<float, float> _maxValue;
        float _minCompare;
        float _maxCompare;

        float _timeInterval = 0.4f;
        // secs
        int _counterStep = 0;
        float _timeCountingSpeed;
        // secs
        const float _maxTimeToCalculateSpeed = 1;
        int _oldCounterStep = 0;

        List<float> _listOfSpeed;
        List<bool> _spikes;
        // 1: max, 0: min

        // Use this for initialization
        void Start()
        {
            init();
            stepCalled += (int step) =>
            {
                    if(stepText != null)
                        stepText.text = "Step: " + step;
                    AudioManager.Instance.PlaySound(AudioManager.AudioSourceType.click);
            };

            speedCalled += (float speed)=>{
                if (speedText != null)
                    speedText.text = "Speed: " + speed + "km/h";
            };
        }
	
        // Update is called once per frame
        void FixedUpdate()
        {
            // adjust threshold, timeInterval
            adjustParameter();

            // detect spike and valley
            detectSpike(getAccelerometerBasedScreenOrientation());


            // reset if too long no detection 
            checkToResetSpike();

            // calculate speed 
            calculateSpeed();
        }


        #region Addition

        void init()
        {
            if (sliderInterval != null)
            {
                sliderInterval.minValue = 0.05f;
                sliderInterval.maxValue = 0.5f;
                sliderInterval.value = _timeInterval;
            }

            if (sliderThresholdHigh != null)
                sliderThresholdHigh.value = 0.2f;
            if (sliderThresholdLow != null)
                sliderThresholdLow.value = -0.2f;
            _minValue = new KeyValuePair<float, float>(Time.time, getLowThreshold());
            _maxValue = new KeyValuePair<float, float>(Time.time, getHighThreshold());
            _spikes = new List<bool>();
            _listOfSpeed = new List<float>();

            _timeCountingSpeed = _maxTimeToCalculateSpeed;

            if (accelerometer != null)
            {
                accelerometer.SetGraphicSize(new Vector2(10, 3), new Vector2(Screen.width, 1000));
                accelerometer.Init();
                accelerometer.EnableDisplay(true);
            }
        }

        void detectSpike(float accX)
        {
            if (accX > getLowThreshold() && accX < getHighThreshold())
            {
                _minCompare = getLowThreshold();
                _maxCompare = getHighThreshold();
            }
            else
            {
                bool change = false;
                if (accX <= _minCompare && accX < getLowThreshold())
                {
                    _minValue = new KeyValuePair<float, float>(Time.time, accX);
                    _minCompare = accX;
                    change = true;
                }
                else if (accX >= _maxCompare && accX > getHighThreshold())
                {
                    _maxValue = new KeyValuePair<float, float>(Time.time, accX);
                    _maxCompare = accX;
                    change = true;
                } 

                if (change)
                {
                    float deltaTime = Mathf.Abs(_minValue.Key - _maxValue.Key);
                    if (deltaTime < _timeInterval)
                    {
                        calculateSpikes();
                   
                        if (_spikes.Count == 3)
                        {
                            _counterStep++;
                            reset();

                            if (stepCalled != null)
                                stepCalled(_counterStep);

                           
                        }
                    }
                }
            }
        }

        void adjustParameter()
        {
            if (sliderInterval != null)
                _timeInterval = sliderInterval.value;
            _threshold = getGyroBasedScreenOrientation();
            if (sliderTextInterval != null)
                sliderTextInterval.text = _timeInterval.ToString();
            if (sliderTextThresholdHigh != null)
                sliderTextThresholdHigh.text = getHighThreshold().ToString();
            if (sliderTextThresholdLow != null)
                sliderTextThresholdLow.text = getLowThreshold().ToString();
        }

        void checkToResetSpike()
        {
            if (_spikes.Count > 0)
            {
                float lastSpikeTimeRecord = (_spikes[1] == true) ? _maxValue.Key : _minValue.Key;
                if (Time.time - lastSpikeTimeRecord > _timeInterval)
                {
                    reset();
                }
            }
        }

        void calculateSpikes()
        {
            if (_minValue.Key < _maxValue.Key)
            {
                if (_spikes.Count == 0)
                {
                    _spikes.Add(false);
                    _spikes.Add(true);
                }
                else if (_spikes.Count == 2)
                {
                    if (!_spikes[1])
                        _spikes.Add(true);
                } 
            }
            else
            {
                if (_spikes.Count == 0)
                {
                    _spikes.Add(true);
                    _spikes.Add(false);
                }
                else if (_spikes.Count == 2)
                {
                    if (_spikes[1])
                        _spikes.Add(false);
                } 
            }

        }

        void calculateSpeed()
        {
            _timeCountingSpeed -= Time.fixedDeltaTime;
            if (_timeCountingSpeed < 0)
            {
                _timeCountingSpeed = _maxTimeToCalculateSpeed;
                int numSteps = _counterStep - _oldCounterStep;
                _oldCounterStep = _counterStep;

                // calculate speed in nearest 5 seconds 
                _listOfSpeed.Add(numSteps * 0.75f);
                if (_listOfSpeed.Count > 5)
                    _listOfSpeed.RemoveAt(0);

                float avgSpeed = 0;
                foreach (float v in _listOfSpeed)
                {
                    avgSpeed += v;
                } 
                avgSpeed /= _listOfSpeed.Count;

                if (speedCalled != null)
                    speedCalled(convertMStoKmH(avgSpeed));

               
            }
        }

        float convertMStoKmH(float number)
        {
            return number * 3.6f; 
        }

        public KeyValuePair<float, float> GetMin()
        {
            return _minValue;
        }

        public KeyValuePair<float, float> GetMax()
        {
            return _maxValue;
        }

        void reset()
        {
            _minValue = new KeyValuePair<float, float>(Time.time, getLowThreshold());
            _maxValue = new KeyValuePair<float, float>(Time.time, getHighThreshold());
            _spikes.Clear();
        }

        float getLowThreshold()
        {
            return  Mathf.RoundToInt((_threshold + ((sliderThresholdLow != null) ? sliderThresholdLow.value : -0.2f)) * 100) / 100f;
        }

        float getHighThreshold()
        {
            return  Mathf.RoundToInt((_threshold + ((sliderThresholdHigh != null) ? sliderThresholdHigh.value : 0.2f)) * 100) / 100f;
        }

        float getGyroBasedScreenOrientation()
        {
            switch (Screen.orientation)
            {
                case ScreenOrientation.Landscape:
                    return Input.gyro.gravity.y;
                case ScreenOrientation.Portrait:
                    return Input.gyro.gravity.x;
            }
            return Input.gyro.gravity.x;
        }

        float getAccelerometerBasedScreenOrientation()
        {
            switch (Screen.orientation)
            {
                case ScreenOrientation.Landscape:
                    return Input.acceleration.y;
                case ScreenOrientation.Portrait:
                    return Input.acceleration.x;
            }
            return Input.acceleration.x;
        }
        #endregion

    }
}