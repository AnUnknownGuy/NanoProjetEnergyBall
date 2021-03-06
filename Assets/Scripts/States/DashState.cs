﻿using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : TemporaryState
{
    public float stunDurationDashInterruptedByWall = 0.2f;
    private float dashStartTimestamp;

    public DashState(Player player, float stunTime) : base(player, stunTime) {
        name = "DashState";
        color = Color.magenta;
    }

    override public void Start() {
        base.Start();
        player.stateManager.numberOfDash++;
        dashStartTimestamp = Time.time;
        player.isDashing = true;
        player.SetSpeed(Vector2.zero);
        player.canDash = false;
        player.SetDashDirection();
        player.StopGravity();
        player.ToFallingLayer();
        player.animator.Play("dash go");
        player.dashCooldownIndicator.Launch();
    }

    public override void Update() {
        player.Dash();
        base.Update();
    }

    public override void Stop() {
        base.Stop();
        player.isDashing = false;
        player.stateManager.timeInDash += Time.time - dashStartTimestamp;
        player.SetSpeed(Vector2.zero);
        player.SetNormalGravity();
        player.ToBaseLayer();

    }


    override public void NextState() {
        if (player.HasBall()) {
            player.ToHoldState();
        } else {
            player.ToBaseState();
        }
    }

    public override void BallEntered(Ball ball) {
        if (player.CatchBall(ball)) {
            player.ToHoldState();
            AudioManager.Ball_Get(player.gameObject);
        }
    }

    public override void WallCollided(Vector2 collisionDirection) {

        if (player.HasBall()) {
            Debug.Log("stuned");
            player.ToHoldStunState(stunDurationDashInterruptedByWall);
        } else {
            Debug.Log("stuned");
            player.ToStunState(stunDurationDashInterruptedByWall);
        }

        player.SetSpeed(collisionDirection * 4);
    }
}
