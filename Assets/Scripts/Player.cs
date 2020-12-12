using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector]
    public StateManager stateManager;

    [HideInInspector]
    public Rigidbody2D rb;

    public InputManager inputManager;

    [Space(10)]
    [Header("Move settings")]
    public float speedWithBall = 5;
    public float speedWithoutBall = 6;
    public float dashPower = 12;
    public float dashDuration = 0.4f;
    public float timeBetweenDash = 1f;
    public float jumpScale = 5;
    public float coyoteTime = 0.2f;
    public float timeBetweenJump = 0.3f;

    public float gravity = 1;
    public float lowGravity = 0.7f;
    public float highGravity = 1.3f;

    [Space(10)]
    [Header("Interaction settings")]

    public float throwPower = 1;
    public float hitStunDuration = 0.8f;
    public float hitSpeedTransfert = 0.8f;

    public float health = 1000;
    public float decay = 10;

    public float healthLostOnDashHit = 50;
    public float healthLostOnBallHit = 20;

    public float timeBeforeBeingAbleToThrow = 0.2f;

    [Space(10)]

    public bool facingRight = true;

    [HideInInspector] public Ball ball;

    [HideInInspector] public Vector2 dashDirection;

    public LayerMask WallsLayer;

    public Vector2 bottomOffset, rightOffset, leftOffset;
    public float collisionRadius = 0.25f, catchRadius = 0.30f;

    [HideInInspector] public bool onGround = false, onPlateform = false, canDash = true, onWallRight = false, onWallLeft = false, isJumping = false, alive = true, isFastFalling = false;

    //Log
    [HideInInspector] public float timeOnGround = 0;
    [HideInInspector] public float timeInAir = 0;
    private float onGroundChangeTimeStamp;
    [HideInInspector] public GameObject lastPlateformTouched;

    public SpriteRenderer sprite;
    public Animator animator;
    public Color color;
    public GameObject deathVFXPrefab;
    public GameObject catchVFXPrefab;
    public float deathDestroyDelay;
    
    public Transform BallTransform;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        stateManager = new StateManager(this);
        onGroundChangeTimeStamp = Time.time;

        if (facingRight) {
            transform.rotation = Quaternion.Euler(0,0,0);
        } else {
            transform.rotation = Quaternion.Euler(0,180,0);
        }
    }



    // Update is called once per frame
    void Update()
    {
        stateManager.Update();

        CheckContactPoints();
        
        AnimJump(isJumping);
        AnimFalling(!onGround);
        AnimRecovery(onGround);
    }

    public void Dash() {

        Vector2 newSpeed = dashDirection * dashPower;
        rb.velocity = newSpeed;


        facingRight = (rb.velocity.x > 0);
        UpdateFacingDirection(0.1f);
    }

    public void Walk(float x)
    {
        if (Mathf.Abs(x) > 0.05f) {
            if (HasBall()) {
                rb.velocity = new Vector2(x * speedWithBall, rb.velocity.y);
            } else {
                rb.velocity = new Vector2(x * speedWithoutBall, rb.velocity.y);
            }

            facingRight = (rb.velocity.x > 0);

            UpdateFacingDirection(0.2f);
            AnimRun(true);
        } else {
            AnimRun(false);
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    public void UpdateFacingDirection(float duration) {

        if (facingRight) {
            //transform.rotation = Quaternion.Euler(0,0,0);

            transform.DORotateQuaternion(Quaternion.Euler(0, 0, 0), duration);

        } else {
            //transform.rotation = Quaternion.Euler(0, 180, 0);
            transform.DORotateQuaternion(Quaternion.Euler(0, 180, 0), duration);
        }

    }

    public void AnimRun(bool bol) {
        if (onGround)
        {
            if (!animator.GetBool("IsRunning") && bol)
                VFXManager.Spawn(VFXManager.Instance.Run, transform.position, facingRight);
            animator.SetBool("IsRunning", bol);
        }
    }


    public void AnimJump(bool bol) {
        if (!animator.GetBool("IsJumping") && bol)
            VFXManager.Spawn(VFXManager.Instance.Jump, transform.position);
        animator.SetBool("IsJumping", bol);
    }

    public void AnimFalling(bool bol) {
        animator.SetBool("IsFalling", bol);
    }

    public void AnimRecovery(bool bol) {
        if (!animator.GetBool("OnGround") && bol)
            VFXManager.Spawn(VFXManager.Instance.FallImpact, transform.position);
        animator.SetBool("OnGround", bol);
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
            ResetJump();
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

    public void ResetJump() {

        canDash = true;
        isJumping = false;
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

    public void PlateformCollided(GameObject plateforme) {
        stateManager.OnPlateformCollided(plateforme);
    }

    public bool CatchBall(Ball ball) {
        if (ball.Catch(this)) {
            this.ball = ball;
            VFXManager.Spawn(catchVFXPrefab, transform.position);
            return true;
        }
        return false;
    }

    public void ThrowBall() {
        if (HasBall()) {
            ball.Throw(inputManager.GetRightStickValue(), throwPower);
            VFXManager.Spawn(VFXManager.Instance.ThrowMuzzle, BallTransform.position);
            Vibration.Vibrate(inputManager.playerInput, 0.5f, 0.2f);

            facingRight = inputManager.GetRightStickValue().x > 0;
            UpdateFacingDirection(0.1f);
            ball = null;
        }
    }

    public void LoseHealth(float amount)
    {
        health -= amount;
        if (health < 10 && alive)
        {
            alive = false;
            CameraManager.Instance.Zoom(transform.position).onComplete += () =>
            {
                VFXManager.Spawn(deathVFXPrefab, transform.position);
                StartCoroutine(Death());
            };
        }
    }

    private IEnumerator Death()
    {
        yield return new WaitForSeconds(deathDestroyDelay);
        Destroy(gameObject);
    }

    public void LoseHealthBallHit() {
        CameraManager.Instance.Shake(0.2f, 0.5f);
        LoseHealth(healthLostOnBallHit);
        Vibration.Vibrate(inputManager.playerInput, 1.0f, 0.2f);
    }

    public void LoseHealthDashHit() {
        CameraManager.Instance.Shake(0.5f, 0.5f);
        LoseHealth(healthLostOnDashHit);
        Vibration.Vibrate(inputManager.playerInput, 1.0f, 0.5f);
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

    public void LoseHealthTick() {
        if (!HasBall())
            LoseHealth(decay * Time.deltaTime);
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

    public void ShowDashNotReady() {
        Color c = sprite.material.color;
        if (c.a >= 1)
            c.a = 0.5f;
        sprite.material.color = c;
    }

    public void ShowDashReady() {
        Color c = sprite.material.color;
        if (c.a < 1)
            c.a = 1;
        sprite.material.color = c;
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
        VFXManager.Spawn(VFXManager.Instance.Dash, transform.position, dashDirection.x > 0);
    }

    private void OnDrawGizmos() {
        
        Gizmos.color = Color.red;


        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        
        if (stateManager != null)
            Gizmos.color = stateManager.currentState.color;

        Gizmos.DrawWireSphere((Vector2)transform.position, 0.25f);
    }
}
