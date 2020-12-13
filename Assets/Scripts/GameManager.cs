using DG.Tweening;
using HealthBarsPackage;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public GameObject levelPrefab;
    public Health health1, health2;
    public LevelManager level;
    public SendLog logger;

    public Image transitionPanel;

    public float fadeTimeIn = 0.5f;
    public float fadeTimeOut = 1f;
    public float timeBlack = 1f;

    public float timeBeforeInputStart;

    [SerializeField] private Point pointP1A;
    [SerializeField] private Point pointP1B;
    [SerializeField] private Point pointP2A;
    [SerializeField] private Point pointP2B;

    public float timeBeforeRestart = 2, timeBeforeCountDown = 3;

    private bool restarting = false;

    void Start()
    {
        timeBeforeInputStart =  timeBeforeCountDown + 3;

        if (level == null) {

            StartCoroutine(StartLevel());
            //AudioManager.Battle_Scene_Stop(gameObject);
            //AudioManager.Battle_Scene(gameObject);
        }
    }

    void Update() {
        if (Input.GetKeyDown("r")) {
            AudioManager.Ambiance_Stop();
            AudioManager.Battle_Scene_Stop();
            SceneManager.LoadScene("BaseLevel");
        }
    }

    public void RestartLevel() {
        if (!restarting) {
            StartCoroutine(Restart());
            restarting = true;
        }
    }

    public void SetHealthBars() {
        health1.player = level.player1;
        health2.player = level.player2;
    }

    
    public void Win(string winner)
    {
        switch (winner)
        {
            case "P1":
                if (!pointP1A.IsActive()) pointP1A.SetBlue();
                else pointP1B.SetBlue();
                break;
            case "P2":
                if (!pointP2A.IsActive()) pointP2A.SetGreen();
                else pointP2B.SetGreen();
                break;
            default: // DRAW
                if (!pointP1A.IsActive()) pointP1A.SetBlue();
                else pointP1B.SetBlue();
                if (!pointP2A.IsActive()) pointP2A.SetGreen();
                else pointP2B.SetGreen();
                break;
        }
        if(pointP1B.IsActive() || pointP2B.IsActive())
        {
            End(winner);
            return;
        }
        RestartLevel();
    }

    public void End(string winner) {
        Debug.Log("winner is :" + winner);

        StartCoroutine(StopLevel());


        AudioManager.Ambiance_Stop();
        AudioManager.Battle_Scene_Stop();
    }

    private IEnumerator Restart() {

        StartCoroutine(StopLevel());

        yield return new WaitForSeconds(fadeTimeIn + timeBeforeRestart);

        StartCoroutine(StartLevel());
    }

    private IEnumerator StartLevel() {
        yield return new WaitForSeconds(timeBlack);
        CreateLevel();
        SetHealthBars();
        restarting = false;

        transitionPanel.DOFade(0, fadeTimeOut);

        yield return new WaitForSeconds(timeBeforeCountDown);
        AudioManager.Countdown(gameObject);
        yield return new WaitForSeconds(3);
        AudioManager.Start_Horn(gameObject);
    }

    private IEnumerator StopLevel() {

        yield return new WaitForSeconds(timeBeforeRestart);
        transitionPanel.DOFade(1, fadeTimeIn);
        yield return new WaitForSeconds(fadeTimeIn);


        if (logger != null) {
            logger.Restart();
        }

        level.Stop();
        Destroy(level.gameObject);
    }

    private void CreateLevel() {
        level = Instantiate(levelPrefab).GetComponent<LevelManager>();
        level.gameManager = this;
    }
    
    
    public static void PauseGame()
    {
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        Time.timeScale = 1;
    }

}
