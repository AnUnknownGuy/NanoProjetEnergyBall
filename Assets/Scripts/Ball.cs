using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    private Rigidbody2D rb;
    private bool sleeping;

    [HideInInspector]
    public Player player;

    public float collisionRadius = 0.25f;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
        Debug.Log("CATCH");
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
        }
    }

    public void Free() {
        this.player = null;
        sleeping = false;
        rb.WakeUp();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.green;


        Gizmos.DrawWireSphere((Vector2)transform.position, collisionRadius);
    }
}
