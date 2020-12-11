using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State {
    public MoveState(Player player) : base(player) {
        name = "MoveState";
    }

    private bool isJumping = false;
    private bool fallingNormal = true;
    private bool fastFalling = false;
    private float coyoteTimer = 0;
    private float dontResetTimerBefore = 0;


    public override void Update() {
        base.Update();

        if (player.onGround) {
            if (player.inputManager.GetLeftStickValue().y > -0.3f) {
                player.ToBaseLayer();
            }

            coyoteTimer = Time.time;
        }


        if (isJumping && player.rb.velocity.y < 0 && !fastFalling) {
            fallingNormal = true;
            player.SetNormalGravity();
            isJumping = false;
        }
    }

    override public bool JumpSignal() {
        if (coyoteTimer + player.coyoteTime > Time.time && player.rb.velocity.y <= 0) {
            AudioManager.Jump(player.gameObject);
            player.stateManager.numberOfJumps++;
            isJumping = true;
            fastFalling = false;
            fallingNormal = false;
            player.Jump();
            player.SetLowGravity();
            return true;
        }
        return false;
    }

    override public bool JumpStopSignal() {
        if (isJumping || player.onGround) {
            player.SetNormalGravity();
            isJumping = false;
            fallingNormal = true;
            fastFalling = false;
            return true;
        }
        return false;
    }

    public override void Stop() {
        base.Stop();
        player.SetNormalGravity();
        player.ToBaseLayer();
        isJumping = false;
    }

    override public void WalkSignal(float x) {
        player.Walk(x);
    }



    public override bool FastFallSignal() {
        if (fallingNormal || isJumping) {
            fallingNormal = false;
            isJumping = false;
        }

        player.stateManager.numberOfFastFall++;
        player.SetHighGravity();
        player.ToFallingLayer();
        fastFalling = true;
        return true;
    }

    public override void WallCollided(Vector2 collisionDirection) {
        player.SetNormalGravity();
    }
}
