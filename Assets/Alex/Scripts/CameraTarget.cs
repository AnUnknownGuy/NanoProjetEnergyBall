using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class CameraTarget : MonoBehaviour
{
    public CinemachineVirtualCamera target;

    public void GoToTarget()
    {
        UITransitionManager.instance.UpdateCamera(target);
    }
}
