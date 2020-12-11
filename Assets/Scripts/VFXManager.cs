using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    
    [Header("VFX Prefabs")]
    public GameObject Dash;
    public GameObject Run;
    public GameObject Jump;
    public GameObject FallImpact;
    public GameObject BlueExplosion;
    public GameObject GreenExplosion;

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    
    
}
