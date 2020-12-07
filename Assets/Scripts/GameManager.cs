using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public LevelManager levelPrefab;
    public LevelManager level;
    public SendLog logger;

    public float timeBeforeRestart = 2;

    private bool restarting = false;

    // Start is called before the first frame update
    void Start()
    {
        if (level == null) {
            CreateLevel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartLevel() {
        if (!restarting) {
            StartCoroutine(Restart(timeBeforeRestart));
            restarting = true;
        }
    }

    public void Win(string winner) {
        if (logger != null) {
            Debug.Log("send !");
            logger.Send(winner);
        }
    }

    private IEnumerator Restart(float time) {
        yield return new WaitForSeconds(time);
        level.Stop();
        Destroy(level.gameObject);
        CreateLevel();
        restarting = false;
        if (logger != null) {
            logger.Restart();
        }
    }

    private void CreateLevel() {
        level = Instantiate(levelPrefab);
        level.gameManager = this;
    }
}
