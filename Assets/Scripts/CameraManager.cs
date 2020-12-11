using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    [SerializeField] private Camera mainCam;

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
        transform.DOComplete();
        transform.DOShakePosition(duration, amount);
        transform.DOShakeRotation(duration, new Vector3(0, amount));
    }

    public void Zoom(Vector3 targetPosition, float zoomDuration = 1.0f, float timeScale = 0.1f, float zoomFactor = 3.0f)
    {
        targetPosition.z = transform.localPosition.z;
        Vector3 initialPos = transform.localPosition;
        float initialSize = mainCam.orthographicSize;
            
        transform.DOLocalMove(targetPosition, zoomDuration * timeScale).SetEase(Ease.OutExpo)
            .OnComplete(() => transform.DOLocalMove(initialPos, zoomDuration).SetEase(Ease.InOutCubic));
        
        mainCam.DOOrthoSize(initialSize/zoomFactor, zoomDuration * timeScale).SetEase(Ease.OutExpo)
            .OnComplete(() => mainCam.DOOrthoSize(initialSize, zoomDuration).SetEase(Ease.OutCubic));
        
        Time.timeScale = timeScale;
        StartCoroutine(ResetTimeScale(zoomDuration, timeScale));
    }

    private IEnumerator ResetTimeScale(float zoomDuration, float timeScale)
    {
        yield return new WaitForSeconds(zoomDuration * timeScale);
        Time.timeScale = 1f;
    }
}
