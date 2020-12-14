using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    public static VFXManager Instance;
    
    [Header("VFX Prefabs")]
    public GameObject Dash;
    public GameObject Run;
    public GameObject Jump;
    public GameObject FallImpact;
    public GameObject BallImpact;
    public GameObject ThrowMuzzle;

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public static void Spawn(GameObject prefab, Vector2 pos, bool flip)
    {
        Spawn(prefab, pos).GetComponent<SpriteRenderer>().flipX = flip;
    }

    public static void Spawn(GameObject prefab, Vector2 pos, Color color, bool isHittingPlayer)
    {
        GameObject FX = Spawn(prefab, pos);
            
        if (isHittingPlayer)
            FX.transform.localScale *= 2f;
        
        VisualEffect VFX = FX.GetComponent<VisualEffect>();
        VFX.SetVector4("Color", color * 5);
        VFX.playRate = 2.5f;
    }

    public static GameObject Spawn(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity);
    }
    
    public static void Spawn(GameObject prefab, Transform parent, bool facingRight)
    {
        GameObject FX = Instantiate(prefab, parent);
        if (facingRight)
            FX.transform.localPosition = Vector3.back/2;
        else 
            FX.transform.localPosition = Vector3.forward/2;
    }
}
