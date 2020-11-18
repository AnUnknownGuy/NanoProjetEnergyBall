using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{

    public Player player;

    private void Start() {

        Debug.Log("ON");
    }

    void OnTriggerEnter(Collider other) {
        Debug.Log("IN");
        player.BallEntered(other.transform.parent.GetComponent<Ball>());
    }
}
