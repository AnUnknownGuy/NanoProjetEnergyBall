using System.Collections;
using System.Collections.Generic;
using System.Runtime.Hosting;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject Activatable;
    
    public void Activate(float delay = 0f)
    {
        StartCoroutine(Activation(delay));
    }

    private IEnumerator Activation(float delay)
    {
        yield return new WaitForSeconds(delay);
        Activatable.SetActive(true);
    }

    public void Disactivate()
    {
        Activatable.SetActive(false);
    }
}
