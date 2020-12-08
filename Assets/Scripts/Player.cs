using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public StateManager stateManager;

    [HideInInspector]
    public Rigidbody2D rb;

    public InputManager inputManager;

    public float speed = 10;
    public float dashPower = 12;
    public float dashDuration = 0.4f;
    public float timeBetweenDash = 1f;
    public float throwPower = 1;
    public float jumpScale = 5;
    public float health = 1000;
    public float decay = 10;
    public float hitStunDuration = 0.8f;
    public float hitSpeedTransfert = 0.8f;

    public float coyoteTime = 0.2f;

    public float gravity = 1;
    public float lowGravity = 0.7f;
    public float highGravity = 1.3f;

    [HideInInspector] public Ball ball;

    [HideInInspector] public Vector2 dashDirection;

    public LayerMask WallsLayer;

    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.25f, catchRadius = 0.30f;

    [HideInInspector] public bool onGround = false, canDash = true, onWallRight = false, onWallLeft = false, isJumping = false, alive = true, isFastFalling = false;

    //Log
    [HideInInspector] public float timeOnGround = 0;
    [HideInInspector] public float timeInAir = 0;
    private float onGroundChangeTimeStamp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateManager = new StateManager(this);
        onGroundChangeTimeStamp = Time.time;
    }

    

    // Update is called once per frame
    void Update()
    {
        stateManager.Update();

        CheckContactPoints();

        alive = !(health < 10);

    }

    public void Dash() {
        Vector2 newSpeed = dashDirection * dashPower;

        rb.velocity = newSpeed;
    }

    public void Walk(float x)
    {
        rb.velocity = new Vector2(x * speed , rb.velocity.y);
    }

    public void Jump() 
    {
        rb.velocity = (new Vector2(0,1) * jumpScale);
        isJumping = true;
    }

    private void CheckContactPoints() 
    {
        bool previousOnground = onGround;

        if (Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, WallsLayer) && rb.velocity.y <= 0) {
            onGround = true;
            canDash = true;
            isJumping = false;
        } else {
            onGround = false;
        }

        if (previousOnground != onGround) {
            if (onGround) {
                timeInAir += Time.time - onGroundChangeTimeStamp;
            } else {
                timeOnGround += Time.time - onGroundChangeTimeStamp;
            }
            onGroundChangeTimeStamp = Time.time;
        }
            
        onWallRight = Physics2D.OverlapCircle((Vector2)transform.position + rightOffset, collisionRadius, WallsLayer);
        onWallLeft = Physics2D.OverlapCircle((Vector2)transform.position + leftOffset, collisionRadius, WallsLayer);
    }

    public void BallEntered(Ball ball) {
        stateManager.OnBallEntered(ball);
    }

    public void DashEntered(Player otherPlayer) {
        stateManager.OnDashEntered(otherPlayer);
    }

    public void WallCollided(Vector2 collisionDirection) {
        stateManager.OnWallCollided(collisionDirection);
    }

    public bool CatchBall(Ball ball) {
        if (ball.Catch(this)) {
            this.ball = ball;
            return true;
        }
        return false;
    }

    public void ThrowBall() {
        if (HasBall()) {
            ball.Throw(inputManager.GetRightStickValue(), throwPower);
            ball = null;
        }
    }

    public void ThrowKnockBack() {
        rb.velocity = -inputManager.GetRightStickValue() * 3;
    }

    public bool HasBall() {
        return ball != null;
    }

    public void ToHoldState() {
        stateManager.ToHold();
    }

    public void ToBaseState() {
        stateManager.ToBase();
    }

    public void ToDashState() {
        stateManager.ToDash();
    }

    public void ToStunState() {
        stateManager.ToStun();
    }

    public void ToStunState(float stunDuration) {
        stateManager.ToStun(stunDuration);
    }

    public void ToHoldStunState(float stunDuration) {
        stateManager.ToHoldStun(stunDuration);
    }

    public void LooseHealth() {
        if (!HasBall())
            health -= decay * Time.deltaTime;
    }
    public void SetSpeed(Vector2 speed) {
        rb.velocity = speed;
    }

    public void SetNormalGravity() {
        //Debug.Log("Normal");
        rb.gravityScale = gravity;
    }
    public void SetLowGravity() {
        //Debug.Log("Low");
        rb.gravityScale = lowGravity;
    }
    public void SetHighGravity() {
        //Debug.Log("High");
        rb.gravityScale = highGravity;
    }
    public void StopGravity() {
        rb.gravityScale = 0;
    }

    public void ToFallingLayer() {
        this.gameObject.layer = LayerMask.NameToLayer("PlayerFalling");
    }

    public void ToBaseLayer() {
        this.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    public void ForceLogUpdate() {
        if (onGround) {
            timeOnGround += Time.time - onGroundChangeTimeStamp;
        } else {
            timeInAir += Time.time - onGroundChangeTimeStamp;
        }
    }

    public void SetDashDirection() {
        dashDirection = inputManager.GetRightStickValue().normalized;
    }

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;


        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + rightOffset, collisionRadius);
        //Gizmos.DrawWireSphere((Vector2)transform.position + leftOffset, collisionRadius);
        
        if (stateManager != null)
            Gizmos.color = stateManager.currentState.color;

        Gizmos.DrawWireSphere((Vector2)transform.position, 0.25f);
    }
}
