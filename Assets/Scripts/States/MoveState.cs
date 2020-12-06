using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State {
    public MoveState(Player player) : base(player) {
        name = "MoveState";
    }

    private bool isJumping = false;
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
        if (player.inputManager.GetLeftStickValue().y < -0.5f) {
            FastFallSignal();
            fastFalling = true;
        } else {
            fastFalling = false;
        }
        if (isJumping && player.rb.velocity.y < 0 && !fastFalling) {
            player.SetNormalGravity();
            isJumping = false;
        }
    }

    override public bool JumpSignal() {
        if (coyoteTimer + player.coyoteTime > Time.time) {
            isJumping = true;
            player.Jump();
            player.SetLowGravity();
            return true;
        }
        return false;
    }

    override public bool JumpStopSignal() {
        if (isJumping) {
            player.SetNormalGravity();
            isJumping = false;
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
        player.SetHighGravity();
        player.ToFallingLayer();
        return true;
    }

    public override void WallCollided(Vector2 collisionDirection) {
        player.SetNormalGravity();
    }
}
