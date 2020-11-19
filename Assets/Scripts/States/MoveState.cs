using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public MoveState(Player player) : base(player) {
        name = "MoveState";
    }

    override public bool JumpSignal() {
        if (player.onGround) {
            player.Jump();
            return true;
        }
        return false;
    }

    override public void WalkSignal(float x) {
        player.Walk(x);
    }


    public override void Update()
    {
        base.Update();
    }
}
