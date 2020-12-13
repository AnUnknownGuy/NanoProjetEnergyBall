using System.Collections;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [SerializeField] private float baseDuration = .1f;
    private float freezeDuration = .1f;
    
    public void Freeze(float duration)
    {
        if(Time.timeScale != 1) return;
        this.freezeDuration = duration;
        Time.timeScale = 0.1f;
        StartCoroutine("Go");
    }

    public void Freeze()
    {
        Freeze(baseDuration);
    }

    private IEnumerator Go()
    {
        yield return new WaitForSecondsRealtime(freezeDuration);
        Time.timeScale = 1;
    }
}
