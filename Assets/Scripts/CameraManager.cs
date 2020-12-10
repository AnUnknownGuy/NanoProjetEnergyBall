using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera mainCam;
    public static CameraManager Instance;
    
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else 
            Destroy(this);

        if (mainCam == null)
            mainCam = GetComponent<Camera>();
    }
    
    public void Shake(float amount, float duration)
    {
        float startTime = Time.time;
        float lastTime = 0;
        
        while (Time.time - startTime < duration)
        {
            if (lastTime + 0.1f < Time.time)
            {
                DoShake(amount);
                lastTime = Time.time;
            }
        }
    }
 
    void DoShake(float amount)
    {
        if (amount > 0)
        {
            Vector3 camPos = mainCam.transform.position;
 
            camPos.x += Random.value * amount * 2 - amount;
            camPos.y += Random.value * amount * 2 - amount;
 
            mainCam.transform.position = camPos;
        }
    }
}
