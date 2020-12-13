using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{

    public Image none;
    public Image blue;
    public Image green;

    public void SetBlue() {
        none.enabled = false;
        blue.enabled = true;
    }

    public void SetGreen() {
        none.enabled = false;
        green.enabled = true;
    }

    public bool IsActive()
    {
        return !none.enabled;
    }

}
