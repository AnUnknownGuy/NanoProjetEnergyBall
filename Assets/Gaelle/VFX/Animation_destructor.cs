using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animation_destructor : MonoBehaviour
{
    private Animation anim;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animation>();
        StartCoroutine(DeleteAfter(anim.clip.length));
    }

    private IEnumerator DeleteAfter(float clipLength)
    {
        yield return new WaitForSeconds(clipLength);
        Destroy(gameObject);
    }
}
