using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFX_destructor : MonoBehaviour
{
    private VisualEffect anim;
    public float lifetime = 5.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<VisualEffect>();
        StartCoroutine(DeleteAfter(lifetime));
    }

    private IEnumerator DeleteAfter(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        Destroy(gameObject);
    }
}
