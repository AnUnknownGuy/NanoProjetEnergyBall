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

    public Point point1;
    public Point point2;
    public Point point3;

    private string winner1, winner2, winner3;
    private int round = 0;
    private bool ended = false;

    public float timeBeforeRestart = 2;
    public float fadeTime = 0.5f;

    private bool restarting = false;

    void Start()
    {
        if (level == null) {
            CreateLevel();
            SetHealthBars();
        }
    }

    void Update() {
        if (Input.GetKeyDown("r")) {
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

    public void Win(string winner) {
        if(round == 0) {
            winner1 = winner;

            if (winner == "P1") {
                point1.SetBlue();
            } else if (winner == "P2") {
                point1.SetGreen();
            } else {
                point1.SetBlue();
                point2.SetGreen();
                round++;
            }

        } else if (round == 1) {
            winner2 = winner;
            if (winner == "P1") {
                point2.SetBlue();
            } else if (winner == "P2") {
                point2.SetGreen();
            } else {

                point2.SetBlue();
                point3.SetGreen();

                End(winner1);
            }
        } else if (round == 2) {

            if (winner == "P1") {
                point3.SetBlue();
            } else if (winner == "P2") {
                point3.SetGreen();
            } else {
                End(winner);
            }

            winner3 = winner;
        }

        if (logger != null) {
            logger.Send(winner);
        }

        if (!ended) {
            if (round == 1) {
                if (winner1 == winner2) {
                    End(winner1);
                } else {
                    RestartLevel();
                }
            } else if (round == 2) {
                End(winner3);
            } else {
                RestartLevel();
            }
        }
        round++;
    }


    public void End(string winner) {
        ended = true;
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
