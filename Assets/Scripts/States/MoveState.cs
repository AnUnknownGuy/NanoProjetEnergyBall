using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : State
{
    public MoveState(Player player) : base(player) {
        name = "MoveState";
    }

    override public void JumpSignal() {
        if (player.onGround)
            player.Jump();
    }

    override public void WalkSignal(float x) {
        player.Walk(x);
    }

    public override void Update()
    {
        base.Update();
        
        if (player.isJumping) player.ProcessJump();
    }
}
