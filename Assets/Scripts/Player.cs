using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private StateManager stateManager;

    private Rigidbody2D rb;

    public float speed = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jump = 5;
    public float health = 1000;
    public float decay = 10;
    [HideInInspector]
    public Ball ball;

    public LayerMask WallsLayer;

    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.25f;

    [HideInInspector]
    public bool onGround = false, onWallRight = false, onWallLeft = false, alive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateManager = new StateManager(this);
    }

    // Update is called once per frame
    void Update()
    {

        stateManager.SendWalk();

        //jump
        if (Input.GetButtonDown("Jump")) {
            stateManager.SendJump();
        }
        //wallJump

        if (Input.GetButtonDown("Jump") && onWallRight) {
           
        }

        if (Input.GetButtonDown("Jump") && onWallLeft) {
            
        }

        ProcessJump();

        stateManager.Update();

        UpdateBools();

        alive = !(health < 10);
    }

    public void Jump() {
        GetComponent<Rigidbody2D>().velocity = (new Vector2(1,1) * jump);
    }

    public void Walk() {
        //A CHANGER C NUL
        float x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(x * speed , rb.velocity.y);
    }

    private void ProcessJump() {

        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        } else if (rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
    private void UpdateBools() {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, WallsLayer);
        onWallRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, WallsLayer);
        onWallLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, WallsLayer);
    }

    public void LooseHealth() {
        health -= decay * Time.deltaTime;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Vector2[] positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
