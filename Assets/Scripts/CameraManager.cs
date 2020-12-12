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
        transform.DOShakePosition(duration, amount);
        transform.DOShakeRotation(duration, new Vector3(0, amount));
    }

    public Tween Zoom(Vector3 targetPosition, float zoomDuration = 1f, float timeScale = 0.1f, float zoomFactor = 3f)
    {
        targetPosition.z = transform.localPosition.z;
        Vector3 initialPos = transform.localPosition;
        float initialSize = mainCam.orthographicSize;
        
        transform.DOLocalMove(targetPosition, zoomDuration * timeScale).SetEase(Ease.OutExpo)
            .OnComplete(() => transform.DOLocalMove(initialPos, zoomDuration).SetEase(Ease.InOutCubic));
        
        Tween zoom = mainCam.DOOrthoSize(initialSize/zoomFactor, zoomDuration * timeScale).SetEase(Ease.OutExpo);
        zoom.OnComplete(() => mainCam.DOOrthoSize(initialSize, zoomDuration).SetEase(Ease.OutCubic));
        
        Time.timeScale = timeScale;
        StartCoroutine(ResetTimeScale(zoomDuration));

        return zoom;
    }

    private IEnumerator ResetTimeScale(float zoomDuration)
    {
        yield return new WaitForSecondsRealtime(zoomDuration);
        Time.timeScale = 1f;
    }
}
