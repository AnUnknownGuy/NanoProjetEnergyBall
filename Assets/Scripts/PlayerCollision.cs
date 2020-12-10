using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public Player player;


    void OnCollisionEnter2D(Collision2D collision) {
        Vector3 normal = collision.contacts[0].normal;
        Vector3 vel = player.rb.velocity;


        if (collision.gameObject.tag == "wall") {

            if (collision.contacts[0].normalImpulse > 5) { // un angle en + aurait été bien m'ais j'ai pas réussi et ça prend trop de temps pour ce que c'est
                player.WallCollided(collision.contacts[0].normal);
            }
        }
    }
}
