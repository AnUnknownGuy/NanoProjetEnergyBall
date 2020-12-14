using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UITransitionManager : MonoBehaviour
{
    public CinemachineVirtualCamera currentCamera;
    public Activator OptionSettings;

    public void Start()
    {
        OptionSettings.Disactivate();
        currentCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        OptionSettings.Disactivate();
        currentCamera.Priority--;
        currentCamera = target;
        currentCamera.Priority++;
        Debug.Log("Next Camera");
    }
}


