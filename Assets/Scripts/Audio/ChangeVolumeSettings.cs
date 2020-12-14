using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeSettings : MonoBehaviour
{
    public Slider thisSlider;
    public float masterVolume;
    public float musicVolume;
    public float SFXVolume;

    public void SetSpecificVolume(string whatValue)
    { 
       float sliderValue = thisSlider.value;

        if (whatValue == "Master")
        {
            Debug.Log("changed master volume to:" + thisSlider.value);
            masterVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MasterVolume", masterVolume);
         
         }

        if (whatValue == "Music")
         {
            // Debug.Log("changed music volume to:" + thisSlider.value);
            musicVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("MusicVolume", musicVolume);
         }

         if (whatValue == "SFX")
        {
            //Debug.Log("changed SFX volume to:" + thisSlider.value);
            SFXVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue("SFXVolume", SFXVolume);
        }

    }

}
