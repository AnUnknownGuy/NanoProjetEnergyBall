using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject levelPrefab;
    public LevelManager level;

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

    private IEnumerator Restart(float time) {
        yield return new WaitForSeconds(time);
        level.Stop();
        Destroy(level.gameObject);
        CreateLevel();
        restarting = false;
    }

    private void CreateLevel() {
        level = Instantiate(levelPrefab).GetComponent<LevelManager>();
        level.gameManager = this;
    }
}
