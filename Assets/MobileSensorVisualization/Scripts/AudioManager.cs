/*******************************************************
 * Copyright (C) 2016 Ngan Do - dttngan91@gmail.com
 *******************************************************/
using UnityEngine;
using System.Collections;

namespace MobileSensor
{
    public class AudioManager : MonoBehaviour
    {

        private static AudioManager _instance;

        public static AudioManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    GameObject go = GameObject.Find("Manager");
                    _instance = go.GetComponent<AudioManager>();
                }
                return _instance;
            }
        }

        public bool isVolumeSound;
        public bool isVolumeMusic;
        public bool isVibrate;
	
        public AudioSource themeMenu;
        public AudioSource themeGame;
        public AudioSource click;

        public enum AudioSourceType
        {
            click
        }

        private bool _firstLoadSound;
        private bool _firstLoadMusic;
        private bool _firstLoadVibrate;

        AudioSource currentTheme;

        void Awake()
        {
            //DontDestroyOnLoad(gameObject);
            //PlayThemeGame();
        }

        #region Sound

        public void PlaySound(AudioSourceType type)
        {
            AudioSource source = convertTOSource(type);
            if (isVolumeSound)
                source.Play();
        }

        public void PlaySoundDynamic(AudioSource source)
        {
            CreateObjectWithSound(source.clip, source.name);
        }

        /// <summary>
        /// Creates the object with sound. Auto destroy after finishing sound
        /// </summary>
        /// <param name="aClip">A clip.</param>
        /// <param name="go">Go.</param>
        void CreateObjectWithSound(AudioClip aClip, string goName)
        {
            // instance a new gameobject
            GameObject apObject = new GameObject(goName);
            // position the object in the world
            apObject.transform.position = Vector3.zero;
            // add an AudioSource component
            apObject.AddComponent<AudioSource>();
            // return this script for use
            AudioSource apAudio = apObject.GetComponent<AudioSource>();
            // initialize some AudioSource fields
            apAudio.playOnAwake = false;
            apAudio.rolloffMode = AudioRolloffMode.Linear;
            apAudio.loop = false;
            apAudio.clip = aClip;
            apAudio.volume = 1;
            // play the clip
            apAudio.Play();
            // destroy this object after clip length
            GameObject.Destroy(apObject, aClip.length);
		
        }

        IEnumerator PlaySoundT(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (isVolumeSound)
                source.Play();
        }

        IEnumerator PlaySoundD(AudioSource source, float delay)
        {
            yield return new WaitForSeconds(delay);
            if (isVolumeSound)
            {
                PlaySoundDynamic(source);
            }
        }

        public void PlaySoundDelay(AudioSource source, float delay)
        {
            StartCoroutine(PlaySoundT(source, delay));
        }

        public void PlaySoundDynamicDelay(AudioSource source, float delay)
        {
            StartCoroutine(PlaySoundD(source, delay));
        }

        AudioSource convertTOSource(AudioSourceType t)
        {
            switch (t)
            {
                case AudioSourceType.click:
                    return click;
            }
            return click;
        }

        #endregion

        #region Music

        public void PlayThemeMenu()
        {
            if (isVolumeMusic)
            {
                themeMenu.Play();
                themeGame.Stop();
                currentTheme = themeMenu;
            }
        }

        public void PlayThemeGame()
        {
            if (isVolumeMusic)
            {
                themeGame.Play();
                themeMenu.Stop();
                currentTheme = themeGame;
            }
        }

        #endregion

        #region Action

        public void Vibrate()
        {
            if (isVibrate)
                Handheld.Vibrate();
        }

        public void ToggleSound()
        {
            isVolumeSound = !isVolumeSound;
        }

        public void ToggleMusic()
        {
            isVolumeMusic = !isVolumeMusic;
            currentTheme.mute = !isVolumeMusic;	
        }

        public void ToggleVibrate()
        {
            isVibrate = !isVibrate;
		
            // demo
            if (isVibrate)
                Handheld.Vibrate();
        }

        #endregion
    }
}