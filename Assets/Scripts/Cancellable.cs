using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cancellable : MonoBehaviour, ICancelHandler
{
    public Selectable cancellationTarget;

    public void OnCancel(BaseEventData eventData)
    {
        cancellationTarget.Select();
    }
}
