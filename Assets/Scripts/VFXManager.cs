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
        FX.GetComponent<VisualEffect>().SetVector4("Color", color * 5);
        if (isHittingPlayer)
            FX.transform.localScale *= 2f;
    }

    public static GameObject Spawn(GameObject prefab, Vector2 pos)
    {
        return Instantiate(prefab, pos, Quaternion.identity);
    }
}
