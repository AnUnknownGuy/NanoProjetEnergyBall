using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MoveState {


    public BaseState(Player player) : base(player) {
        name = "BaseState";
        color = Color.green;
    }

    override public void Update() {
        base.Update();
        player.LooseHealth();
    }

    public override bool ActionSignal() {
        if (player.canDash) {
            player.ToDashState();
            return true;
        } else {
            return false;
        }
    }


}
