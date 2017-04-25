/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace MobileSensor
{
    public class SensorGraphics : MonoBehaviour
    {
        public SensorManager.SensorType currentSensor;

        public Text sensorValue;
        public GameObject prefabPoint;
        public GameObject prefabGrid;
        public Transform pivotList;
        public Transform pivotGrid;


        List<Vector3> _dataList;
        Vector2 _sizeScreen = new Vector2(1080, 500);
        Vector2 _sizeCell = new Vector2(10, 3);
        Transform[] _displayPointsX;
        Transform[] _displayPointsY;
        Transform[] _displayPointsZ;
        bool _isDisplayed = false;

        #region Unity messages

        // Use this for initialization
        void Start()
        {
        }
	
        // Update is called once per frame
        void FixedUpdate()
        {
            // title
            titleDisplay();

            // data 
            updateData();
       
            // draw graphics
            if (_isDisplayed)
            {
                fromDataToGraphic();
            }
            pivotList.gameObject.SetActive(_isDisplayed);
        }

        #endregion

        #region retrieve Datapub
        public void SetGraphicSize(Vector2 cellSize, Vector2 screenSize)
        {
            _sizeCell = cellSize;
            _sizeScreen = screenSize;
        }

        public void EnableDisplay(bool flag)
        {
            _isDisplayed = flag;
        }

        #endregion

        #region Logic

        public void Init()
        {
            _dataList = new List<Vector3>();
            _displayPointsX = new Transform[Mathf.RoundToInt(_sizeScreen.x / _sizeCell.x)];
            _displayPointsY = new Transform[Mathf.RoundToInt(_sizeScreen.x / _sizeCell.x)];
            _displayPointsZ = new Transform[Mathf.RoundToInt(_sizeScreen.x / _sizeCell.x)];

            for (int i = 0; i < _displayPointsX.Length; i++)
            {
                GameObject go = GameObject.Instantiate(prefabPoint);
                go.transform.SetParent(pivotList);
                go.SetActive(false);
                go.GetComponent<Image>().color = Color.red;
                _displayPointsX[i] = go.transform;
            }

            for (int i = 0; i < _displayPointsY.Length; i++)
            {
                GameObject go = GameObject.Instantiate(prefabPoint);
                go.transform.SetParent(pivotList);
                go.SetActive(false);
                go.GetComponent<Image>().color = Color.green;

                _displayPointsY[i] = go.transform;
            }

            for (int i = 0; i < _displayPointsZ.Length; i++)
            {
                GameObject go = GameObject.Instantiate(prefabPoint);
                go.transform.SetParent(pivotList);
                go.SetActive(false);
                go.GetComponent<Image>().color = Color.blue;

                _displayPointsZ[i] = go.transform;
            }

            int sizeCellY = 3;
            int cellGridY = Mathf.FloorToInt(_sizeScreen.y / (sizeCellY * 2));
            for (int i = -sizeCellY; i <= sizeCellY; i++)
            {
                GameObject go = GameObject.Instantiate(prefabGrid);
                go.transform.SetParent(pivotGrid);
                go.transform.localPosition = new Vector3(0, i * cellGridY, 0);
                go.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 5);
            }
        }

        void updateData()
        {
            _dataList.Add(getCurrentSensorData());
            if (_dataList.Count > _displayPointsX.Length)
            {
                _dataList.RemoveAt(0);
            }
        }

        void resetData()
        {
            _dataList.Clear();
            foreach (Transform t in _displayPointsX)
            {
                t.localPosition = Vector3.zero;
            }
            foreach (Transform t in _displayPointsY)
            {
                t.localPosition = Vector3.zero;
            }
            foreach (Transform t in _displayPointsZ)
            {
                t.localPosition = Vector3.zero;
            }
        }

        void fromDataToGraphic()
        {
            int current = _dataList.Count - 1;
            foreach (Vector3 v in _dataList)
            {
                float x = current * _sizeCell.x;
                _displayPointsX[current].localPosition = new Vector3(x, convertToPositionGraphY(v.x), 0);
                _displayPointsY[current].localPosition = new Vector3(x, convertToPositionGraphY(v.y), 0);
                _displayPointsZ[current].localPosition = new Vector3(x, convertToPositionGraphY(v.z), 0);

                _displayPointsX[current].gameObject.SetActive(true);
                _displayPointsY[current].gameObject.SetActive(true);
                _displayPointsZ[current].gameObject.SetActive(true);
                current--;
            }
        }

        float convertToPositionGraphY(float y)
        {
            int height = Mathf.RoundToInt(_sizeScreen.y / 2);
            return y * height / _sizeCell.y;
        }

        Vector3 getCurrentSensorData()
        {
            switch (currentSensor)
            {
                case SensorManager.SensorType.Accelerometer:
                    return Input.acceleration;
                    break;
                case SensorManager.SensorType.Gyroscope:
                    return Input.gyro.gravity;
                    break;
                case SensorManager.SensorType.Compass:
                    return Input.compass.rawVector;
                    break;
            }
            return Vector3.zero;
        }

        #endregion

        #region Display

        void titleDisplay()
        {
            sensorValue.text = getCurrentSensorData().ToString();
        }

        #endregion
    }
}