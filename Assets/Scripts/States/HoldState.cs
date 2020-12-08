using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : MoveState
{


    private float holdStartTimestamp;

    public HoldState(Player player, AudioManager audioManager) : base(player, audioManager) {
        name = "HoldState";
        color = Color.blue;
    }

    public override void Start() {
        base.Start();
        player.stateManager.numberOfBallCatched++;
        holdStartTimestamp = Time.time;

    }

    public override void Stop() {
        base.Stop();
        player.stateManager.timeInHold = Time.time - holdStartTimestamp;
    }
    public override bool ActionSignal() {
        audioManager.Throw_ball.Post(player.gameObject);
        player.ThrowBall();
        player.ThrowKnockBack();
        player.ThrowKnockBack();
        player.ToStunState();
        return true;
    }

}
