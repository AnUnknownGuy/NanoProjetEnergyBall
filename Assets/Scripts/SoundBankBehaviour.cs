#if UNITY_EDITOR
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class SoundBankBehaviour
{
    public AK.Wwise.Bank bank;

    public bool asynchronous;
    public bool decodeCompressedData;

    public bool loadOnAwake;

    [Space]
    public SceneAsset loadingScene;
    public SceneAsset unloadingScene;

    [Space]
    public float loadingDelay;
    public float unloadingDelay;

    [HideInInspector] public bool isLoaded;

    /*
    public IEnumerator LoadBank()
    {
        if (!isLoaded)
        {
            while (true)
            {
                yield return new WaitForSeconds(loadingDelay);
                isLoaded = true;
                Debug.Log("Bank " + Bank + " loaded.");
            }
        }
    }
    */

    public void LoadBank()
    {
        if (!isLoaded)
        {
            isLoaded = true;

            if (asynchronous)
                bank.LoadAsync();

            if (decodeCompressedData)
                //AkSoundEngine.LoadAndDecodeBank(bank.Name, false, out uint bankID);
                bank.Load(true, false);
            else
                //AkSoundEngine.LoadBank(bank.Name, out uint bankID);
                bank.Load(false, false);
            Debug.Log("Bank " + bank.Name + " loaded.");
        }
        else
            Debug.LogWarning("Bank " + bank.Name + " already loaded. Aborting loading.");
    }

    /*
    public IEnumerator UnloadBank()
    {
        if (isLoaded)
        {
            while (true)
            {
                yield return new WaitForSeconds(loadingDelay);
                isLoaded = false;
                Debug.Log("Bank " + Bank + " unloaded.");
            }
        }
    }
    */

    public void UnloadBank()
    {
        if (isLoaded)
        {
            isLoaded = false;

            bank.Unload();

            Debug.Log("Bank " + bank.Name + " unloaded.");
        }
        else
            Debug.LogWarning("Bank " + bank.Name + " not loaded. Aborting unloading.");
    }
}

#endif