using UnityEngine;

public class PlayerBallCollider : MonoBehaviour
{

    public Player player;

    void OnTriggerStay2D(Collider2D other) {
        player.BallEntered(other.GetComponent<Ball>());
    }
}
