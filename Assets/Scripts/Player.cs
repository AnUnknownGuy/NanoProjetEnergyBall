using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float inputThreshold = 0.5f;
    
    private StateManager stateManager;
    private PlayerControls playerControls;

    private Rigidbody2D rb;

    public float speed = 10;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float jumpScale = 5;
    public float health = 1000;
    public float decay = 10;

    [HideInInspector]
    public Ball ball;

    public LayerMask WallsLayer;

    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.25f, catchRadius = 0.30f;

    [HideInInspector] public bool onGround = false, onWallRight = false, onWallLeft = false, isJumping = false, alive = true;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateManager = new StateManager(this);
        BindControls();
    }

    void BindControls()
    {
        playerControls = new PlayerControls();
        playerControls.Controls.Move.performed += stateManager.OnMove;
        playerControls.Controls.Rightstick.performed += stateManager.OnRightStick;

        playerControls.Controls.Move.Enable();
        playerControls.Controls.Rightstick.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        stateManager.Update();

        CheckContactPoints();

        alive = !(health < 10);
    }

    public void Walk(float x)
    {
        rb.velocity = new Vector2(x * speed , rb.velocity.y);
    }

    public void Jump() 
    {
        GetComponent<Rigidbody2D>().velocity = (new Vector2(0,1) * jumpScale);
        isJumping = true;
    }

    public void ProcessJump() 
    {
        if (rb.velocity.y < 0) {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime);
        } else if (rb.velocity.y > 0 && playerControls.Controls.Move.ReadValue<Vector2>().y > inputThreshold) {
            rb.velocity += Vector2.up * (Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime);
        }
    }
    private void CheckContactPoints() 
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, WallsLayer);
        isJumping &= !onGround;
        onWallRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, WallsLayer);
        onWallLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, WallsLayer);
    }

    public void BallEntered(Ball ball) {
        stateManager.SendBallEntered(ball);
    }

    public void CatchBall(Ball ball) {
        if (ball.Catch(this)) {
            this.ball = ball;
        }
    }

    public void ToHoldState() {
        stateManager.ToHold();
    }

    public void ToBaseState() {
        stateManager.ToBase();
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
