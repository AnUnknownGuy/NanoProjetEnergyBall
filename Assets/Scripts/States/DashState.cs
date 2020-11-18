using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : TemporaryState
{
    public DashState(Player player, float stunTime) : base(player, stunTime) {
        name = "DashState";
    }

    public override void BallEntered(Ball ball) {
        player.CatchBall(ball);
    }

    override public void NextState() {
        if (player.HasBall()) {
            player.ToHoldState();
        } else {
            player.ToBaseState();
        }

    }
}
