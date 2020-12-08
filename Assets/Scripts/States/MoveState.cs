using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public MoveState(Player player) : base(player) {
        name = "MoveState";
    }


    public override void Update() {
        base.Update();
        if (player.onGround) {
            player.SetNormalGravity();
            if (player.inputManager.GetLeftStickValue().y > -0.3f) {
                player.ToBaseLayer();
            }
        }
        if (player.inputManager.GetLeftStickValue().y < -0.3f) {
            FastFallSignal();
        }
    }

    override public bool JumpSignal() {
        if (player.onGround) {
            player.Jump();
            return true;
        }
        return false;
    }

    public override void Stop() {
        base.Stop();
        player.SetNormalGravity();
        player.ToBaseLayer();
    }

    override public void WalkSignal(float x) {
        player.Walk(x);
    }



    public override bool FastFallSignal() {
        player.SetHighGravity();
        player.ToFallingLayer();
        return true;
    }
}
