using System;
using System.Collections;
using System.Collections.Generic;
using AK.Wwise;
using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeSettings : MonoBehaviour
{
    public Slider thisSlider;
    public string OptionName;
    private float rtpcValue;
    private int type = (int)AkQueryRTPCValue.RTPCValue_Global;

    private static float master = 100;
    private static float music = 100;
    private static float SFX = 100;

    private void Start()
    {
        // AKRESULT result = AkSoundEngine.GetRTPCValue(OptionName, null, 0, out rtpcValue, ref type);
        // if (result == AKRESULT.AK_Success)
        //     thisSlider.value = rtpcValue;
        // else 
        //     Debug.LogWarning(result.ToString());

        switch (OptionName)
        {
            case "Master":
                thisSlider.value = master;
                break;
            case "Music":
                thisSlider.value = music;
                break;
            case "SFX":
                thisSlider.value = SFX;
                break;
        }
    }

    public void SetVolume()
    {
        AkSoundEngine.SetRTPCValue(OptionName + "Volume", thisSlider.value);
        
        switch (OptionName)
        {
            case "Master":
                master = thisSlider.value;
                break;
            case "Music":
                music = thisSlider.value;
                break;
            case "SFX":
                SFX = thisSlider.value;
                break;
        }
    }
}
