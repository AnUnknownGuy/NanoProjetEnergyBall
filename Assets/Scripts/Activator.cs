using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject Activatable;

    public void OnTriggerEnter(Collider other)
    {
        Activatable.SetActive(true);
    }

    public void OnTriggerExit(Collider other)
    {
        Activatable.SetActive(false);
    }
}
