using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selector : MonoBehaviour
{
    public Selectable selectable;

    public void SelectTarget()
    {
        if (gameObject == EventSystem.current.currentSelectedGameObject)
            selectable.Select();
    }
}