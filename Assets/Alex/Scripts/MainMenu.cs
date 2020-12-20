using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private GameObject selectedItem;

    private void Start()
    {
        selectedItem = EventSystem.current.firstSelectedGameObject;
    }

    private void Update()
    {
        if (!EventSystem.current.currentSelectedGameObject)
        {
            selectedItem.GetComponent<Selectable>().Select();
        }
        else if (!Equals(selectedItem, EventSystem.current.currentSelectedGameObject))
        {
            selectedItem = EventSystem.current.currentSelectedGameObject;
            selectedItem?.GetComponent<CameraTarget>()?.GoToTarget();
        }
        //if (EventSystem.)
    }

    public void PlayGame ()
    {
        AudioManager.Begin(gameObject);
        SceneManager.LoadScene("BaseLevel");
    }

    public void Validate()
    {
        AudioManager.Validate(gameObject);
    }

    public void QuitGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
 
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Debug.Log("Quit");
    }
}
