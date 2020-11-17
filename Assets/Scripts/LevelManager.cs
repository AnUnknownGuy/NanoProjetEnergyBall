using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public Slider healthPlayer1;
    public Slider healthPlayer2;
    public Player player1;
    public Player player2;

    public GameManager gameManager;

    private bool restarting = false;

    // Start is called before the first frame update
    void Start()
    {
        healthPlayer1.maxValue = player1.health;
        healthPlayer2.maxValue = player2.health;
    }

    // Update is called once per frame
    void Update() {

        if (!restarting) {
            healthPlayer1.value = player1.health;
            healthPlayer2.value = player2.health;

            if (!player1.alive && !player2.alive) {
                Debug.Log("DRAW !");
            } else if (!player1.alive) {
                Debug.Log("PLAYER 2 WINS !");
            } else if (!player2.alive) {
                Debug.Log("PLAYER 1 WINS !");
            }

            if (!player1.alive || !player2.alive) {
                gameManager.RestartLevel();
                restarting = true;
            }
        }
        
    }
}
