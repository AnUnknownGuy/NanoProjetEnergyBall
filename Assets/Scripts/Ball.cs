using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool sleeping;
    private float timerUntilGravityChange;
    private bool thrown = false;

    public float gravityWhenThorwn = 0.2f;
    public float gravity = 2f;

    [HideInInspector]
    public Player player;

    public float collisionRadius = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(thrown.ToString()); // TODO: Remove this once this.thrown is used somewhere else.
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
        if (!sleeping) {
            Free();
            rb.velocity = Vector2.zero;
            rb.velocity += dir.normalized * force;
        }
    }

    public void Free() {
        this.player = null;
        sleeping = false;
        rb.WakeUp();
    }

    private void OnDrawGizmos() {
        //Gizmos.color = Color.green;


        //Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }
}
