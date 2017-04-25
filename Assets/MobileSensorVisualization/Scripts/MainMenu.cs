/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MobileSensor
{
    public class MainMenu : MonoBehaviour {

        public enum SensorMenu{
            SensorGraphics,
            StepCounter,
            CompassDemo,
            MainMenu
        }
        SensorMenu _currentMenu;

        public Button sensorDemoBtn;
        public Button stepCounterDemoBtn;
        public Button compassDemoBtn;
        public Button planeControllerBtn;
        public Button backDemoBtn;

    	// Use this for initialization
    	void Start () {
            DontDestroyOnLoad(gameObject);

            _currentMenu = SensorMenu.MainMenu;
            sensorDemoBtn.onClick.RemoveAllListeners();
            sensorDemoBtn.onClick.AddListener(() =>
                {
                    _currentMenu = SensorMenu.SensorGraphics;
                    SceneManager.LoadScene("SensorGraphics");
                });

            stepCounterDemoBtn.onClick.RemoveAllListeners();
            stepCounterDemoBtn.onClick.AddListener(() =>
                {
                    _currentMenu = SensorMenu.StepCounter;
                    SceneManager.LoadScene("StepCounter");
                });

            compassDemoBtn.onClick.RemoveAllListeners();
            compassDemoBtn.onClick.AddListener(() =>
                {
                    _currentMenu = SensorMenu.CompassDemo;
                    SceneManager.LoadScene("CompassDevice");
                });
            planeControllerBtn.onClick.RemoveAllListeners();
            planeControllerBtn.onClick.AddListener(() =>
                {
                    _currentMenu = SensorMenu.CompassDemo;
                    SceneManager.LoadScene("PlaneController");
                });
            backDemoBtn.onClick.RemoveAllListeners();
            backDemoBtn.onClick.AddListener(() =>
                {
                    _currentMenu = SensorMenu.MainMenu;
                    SceneManager.LoadScene("MainMenu");
                });
    	}
    	
    	// Update is called once per frame
    	void Update () {
            sensorDemoBtn.gameObject.SetActive(_currentMenu == SensorMenu.MainMenu);
            stepCounterDemoBtn.gameObject.SetActive(_currentMenu == SensorMenu.MainMenu);
            compassDemoBtn.gameObject.SetActive(_currentMenu == SensorMenu.MainMenu);
            planeControllerBtn.gameObject.SetActive(_currentMenu == SensorMenu.MainMenu);
            backDemoBtn.gameObject.SetActive(_currentMenu != SensorMenu.MainMenu);
    	}

    }
}