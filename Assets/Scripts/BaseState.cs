using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : State {


    public BaseState(Player player) : base(player) {
        name = "BaseState";
    }

    override public void JumpSignal() {
        if (player.onGround)
            this.player.Jump();
    }

    override public void WalkSignal() {
        this.player.Walk();
    }

    override public void Update() {
        player.LooseHealth();
    }

}
