using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{

    public Player player;

    private void OnTriggerEnter(Collider other) {
        player.BallEntered(other.transform.parent.GetComponent<Ball>());
    }
}
