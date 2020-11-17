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

    // Start is called before the first frame update
    void Start()
    {
        healthPlayer1.maxValue = player1.health;
        healthPlayer2.maxValue = player2.health;
    }

    // Update is called once per frame
    void Update() {
        healthPlayer1.value = player1.health;
        healthPlayer2.value = player2.health;
    }
}
