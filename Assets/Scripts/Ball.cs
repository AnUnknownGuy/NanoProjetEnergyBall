using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    
    private Rigidbody2D rb;

    public PhysicsMaterial2D material;

    private bool sleeping;
    private float timerUntilGravityChange;


    public float gravityWhenThorwn = 0.2f;
    public float gravity = 2f;

    [HideInInspector]
    public Player player;

    [HideInInspector]
    public bool charged = false;
    [HideInInspector]
    public Player previousPlayer;

    public float collisionRadius = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = gravity;
        sleeping = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) {
            transform.position = player.transform.position;
        }
    }

    public bool Catch(Player player) {
        if (!sleeping) {
            this.player = player;
            sleeping = true;
            rb.Sleep();
            return true;
        }else {
            return false;
        }
    }

    public void Throw(Vector2 dir, float force) {
            Charge();
            Free();
            rb.velocity = Vector2.zero;
            rb.velocity += dir.normalized * force;
    }

    public void Free() {
        player = null;
        sleeping = false;
        rb.WakeUp();
    }

    public void Charge() {
        previousPlayer = player;
        rb.gravityScale = 0;
        charged = true;
    }

    public void Uncharge() {
        charged = false;
        rb.gravityScale = gravity;
    }

    public void MoveToPreviousPlayer() {

    }

    public void Hit() {
        Uncharge();
        MoveToPreviousPlayer();
    }

    public void SetSpeed(Vector2 speed) {
        rb.velocity = speed;
    }

    public void FakeCollision() {
        rb.velocity = -rb.velocity * material.bounciness;
    }

    public Vector2 GetSpeed() {
        return rb.velocity;
    }

    private void OnDrawGizmos() {
        //Gizmos.color = Color.green;


        //Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        Vector3 normal = collision.contacts[0].normal;
        
        if (charged) {
            if (collision.gameObject.tag == "wall") {
                Hit();
            }
        }
    }

}
