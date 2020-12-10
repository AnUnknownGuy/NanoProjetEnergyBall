#if UNITY_EDITOR
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundbanksManager : MonoBehaviour
{
    public static SoundbanksManager instance;

    private void Awake()
    {
        if (instance) Destroy(this);
        else instance = this;

        Init();
    }

    public SoundBankBehaviour[] soundBanks;

    //public GameObject bankLoaderTimer;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode arg1)
    {
        foreach (SoundBankBehaviour soundBank in soundBanks)
        {
            if (scene.name == soundBank.loadingScene.name && !soundBank.isLoaded)
            {
                StartCoroutine(BankProcessDelay(soundBank, true));
            }
            else if (scene.name == soundBank.unloadingScene.name && soundBank.isLoaded)
            {
                StartCoroutine(BankProcessDelay(soundBank, false));
            }
        }
    }

    public void Init()
    {
        foreach (SoundBankBehaviour soundBank in soundBanks)
            soundBank.isLoaded = false;

        //AkSoundEngine.LoadBank("Init", out uint bankID);

        //Debug.Log("BanksManager Enable");

        foreach (SoundBankBehaviour soundBank in soundBanks)
        {
            if (soundBank.loadOnAwake)
                soundBank.LoadBank();
        }
    }

    public void LoadBank(AK.Wwise.Bank in_soundBank)
    {
        SoundBankBehaviour bankToLoad = null;

        foreach (SoundBankBehaviour soundBank in soundBanks)
            if (soundBank.bank == in_soundBank)
                bankToLoad = soundBank;

        StartCoroutine(BankProcessDelay(bankToLoad, true));
    }

    public void UnloadBank(AK.Wwise.Bank in_soundBank)
    {
        SoundBankBehaviour bankToUnload = null;

        foreach (SoundBankBehaviour soundBank in soundBanks)
            if (soundBank.bank == in_soundBank)
                bankToUnload = soundBank;

        StartCoroutine(BankProcessDelay(bankToUnload, false));
    }

    public IEnumerator BankProcessDelay(SoundBankBehaviour bank, bool toLoad)
    {
        float delay = 0;

        if (toLoad)
            delay = bank.loadingDelay;
        else
            delay = bank.unloadingDelay;

        yield return new WaitForSeconds(delay);

        if (toLoad)
            bank.LoadBank();
        else
            bank.UnloadBank();

        Destroy(gameObject);
    }
}

#endif