using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class UITransitionManager : MonoBehaviour
{
    public static UITransitionManager instance;
    
    public CinemachineVirtualCamera currentCamera;

    public void Start()
    {
        instance = this;
        currentCamera.Priority++;
    }

    public void UpdateCamera(CinemachineVirtualCamera target)
    {
        currentCamera.Priority--;
        currentCamera = target;
        currentCamera.Priority++;
        Debug.Log("Next Camera");
    }
}


