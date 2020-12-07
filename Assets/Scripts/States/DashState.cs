using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : TemporaryState
{
    public float stunDurationDashInterruptedByWall = 0.2f;

    public DashState(Player player, float stunTime) : base(player, stunTime) {
        name = "DashState";
        color = Color.magenta;
    }

    override public void Start() {
        base.Start();
        player.SetSpeed(Vector2.zero);
        player.canDash = false;
        player.SetDashDirection();
        player.StopGravity();
    }

    public override void Update() {
        player.Dash();
        base.Update();
    }

    public override void Stop() {
        base.Stop();
        player.SetSpeed(Vector2.zero);
        player.SetNormalGravity();

    }

    override public void BallEntered(Ball ball) {
        player.CatchBall(ball);
    }

    override public void NextState() {
        if (player.HasBall()) {
            player.ToHoldState();
        } else {
            player.ToBaseState();
        }
    }

    public override void WallCollided(Vector2 collisionDirection) {

        if (player.HasBall()) {
            player.ToHoldStunState(stunDurationDashInterruptedByWall);
        } else {
            player.ToStunState(stunDurationDashInterruptedByWall);
        }

        player.SetSpeed(collisionDirection * 4);
    }
}
