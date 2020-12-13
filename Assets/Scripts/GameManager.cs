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
    public Image greyPanel;
    public Image victoryImageBlue;
    public Image victoryImageGreen;
    public Button replayButton;
    public Button mainMenuButton;
    public Text replayText;
    public Text mainMenuText;

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

    private Image victoryImage;

    public Image count1;
    public Image count2;
    public Image count3;
    public Image go;

    void Start()
    {
        timeBeforeInputStart =  timeBeforeCountDown + 3;

        if (level == null) {
            StartCoroutine(StartLevel());
        }
    }

    void Update() {
        if (Input.GetKeyDown("r")) {
            RestartScene();
        }
    }

    public void RestartScene() {
        StartCoroutine(CoRestartScene());
    }

    private IEnumerator CoRestartScene() {

        AudioManager.Ambiance_Stop();
        AudioManager.Battle_Scene_Stop();

        transitionPanel.DOFade(1, fadeTimeIn);
        yield return new WaitForSeconds(fadeTimeIn);
        SceneManager.LoadScene("BaseLevel");
    }

    public void ToMainMenu() {
        StartCoroutine(CoToMainMenu());
    }

    private IEnumerator CoToMainMenu() {
        AudioManager.Ambiance_Stop();
        AudioManager.Battle_Scene_Stop();

        transitionPanel.DOFade(1, fadeTimeIn);
        yield return new WaitForSeconds(fadeTimeIn);
        SceneManager.LoadScene("MainMenuAlex");
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
        AudioManager.Ambiance_Stop();
        AudioManager.Battle_Scene_Stop();


        if (winner == "P1") {
            AudioManager.Win_Blue(gameObject);
            victoryImage = victoryImageBlue;
        } else {
            AudioManager.Win_Green(gameObject);
            victoryImage = victoryImageGreen;
        }


        StartCoroutine(ShowVictory());
    }

    private IEnumerator ShowVictory() {

        yield return new WaitForSeconds(1f);

        greyPanel.DOFade(0.75f, 0.5f);

        yield return new WaitForSeconds(0.5f);

        AudioManager.Win(gameObject);
        yield return new WaitForSeconds(0.15f);

        victoryImage.DOFade(1f, 0);

        yield return new WaitForSeconds(1);
        replayButton.interactable = true;
        mainMenuButton.interactable = true;
        replayButton.image.DOFade(1, 1);
        mainMenuButton.image.DOFade(1, 1);
        replayText.DOFade(1, 1);
        mainMenuText.DOFade(1, 1);
    }

    private IEnumerator Restart() {

        StartCoroutine(StopLevel());
        transitionPanel.DOFade(1, fadeTimeIn);

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
        StartCoroutine(ShowCountDown());
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
    
    private IEnumerator ShowCountDown() {
        StartCoroutine(ShowOneCount(count3));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowOneCount(count2));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowOneCount(count1));
        yield return new WaitForSeconds(1);
        StartCoroutine(ShowGo(go));
    }

    private IEnumerator ShowOneCount(Image image) {
        image.enabled = true;
        image.DOFade(0, 1.0f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(1);
        image.enabled = false;
        image.DOFade(1, 0);
    }

    private IEnumerator ShowGo(Image image) {
        image.enabled = true;
        image.DOFade(0, 0.5f).SetEase(Ease.InCubic);
        image.transform.DOScale(3, 0.5f).SetEase(Ease.InCubic);
        yield return new WaitForSeconds(0.5f);
        image.enabled = false;
        image.DOFade(1, 0);
        image.transform.localScale = new Vector3(0.5f, 0.5f, 1);
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
