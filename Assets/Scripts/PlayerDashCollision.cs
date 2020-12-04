using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashCollision : MonoBehaviour
{
    public Player player;

    void OnTriggerStay2D(Collider2D other) {
        if (other.tag == "Player") {
            Player otherPlayer = other.GetComponent<PlayerDashCollision>().player;
            if (otherPlayer.stateManager.currentState.GetName() == "DashState")
                player.DashEntered(otherPlayer);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "wall") {

        }
    }

}
