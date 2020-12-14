using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        GetComponentInChildren<Button>().Select();
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
