using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rb;

    public float speed = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jump = 5;

    public LayerMask WallsLayer;

    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.25f;
    private bool onGround = false, onWallRight = false, onWallLeft = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Walk();

        //jump
        if (Input.GetButtonDown("Jump") && onGround) {
            GetComponent<Rigidbody2D>().velocity += Vector2.up * jump;
        }
        //wallJump

        if (Input.GetButtonDown("Jump") && onWallRight) {
           
        }

        if (Input.GetButtonDown("Jump") && onWallLeft) {
            
        }

        ProcessJump();


        UpdateBools();
    }

    private void Walk() {
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

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;

        Vector2[] positions = new Vector2[] { bottomOffset, rightOffset, leftOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
    }
}
