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

    private int p1win = 0;
    private int p2win = 0;
    [SerializeField] private Point pointP1A;
    [SerializeField] private Point pointP1B;
    [SerializeField] private Point pointP2A;
    [SerializeField] private Point pointP2B;

    public float timeBeforeRestart = 2;
    public float fadeTime = 0.5f;

    private bool restarting = false;

    void Start()
    {
        if (level == null) {
            CreateLevel();
            SetHealthBars();

            //AudioManager.Battle_Scene_Stop(gameObject);
            //AudioManager.Battle_Scene(gameObject);
        }
    }

    void Update() {
        if (Input.GetKeyDown("r")) {
            AudioManager.Battle_Scene_Stop();
            SceneManager.LoadScene("BaseLevel");
        }
    }

    public void RestartLevel() {
        if (!restarting) {
            AudioManager.Start_Horn(gameObject);
            StartCoroutine(Restart(fadeTime));
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
                p1win++;
                if (!pointP1A.IsActive()) pointP1A.SetBlue();
                else pointP1B.SetBlue();
                break;
            case "P2":
                p2win++;
                if (!pointP2A.IsActive()) pointP2A.SetGreen();
                else pointP2B.SetGreen();
                break;
            default: // DRAW
                p1win++;
                p2win++;
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
    }

    private IEnumerator Restart(float time) {

        transitionPanel.DOFade(1, time/2);
        yield return new WaitForSeconds(time);

        level.Stop();
        Destroy(level.gameObject);
        CreateLevel();
        SetHealthBars();
        restarting = false;
        if (logger != null) {
            logger.Restart();
        }

        transitionPanel.DOFade(0, time / 2);
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
