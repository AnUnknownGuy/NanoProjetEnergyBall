using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : MoveState
{

    public HoldState(Player player): base(player) {
        name = "HoldState";
        color = Color.blue;
    }

    public override bool ActionSignal() {
        player.ThrowBall();
        player.ToStunState();
        return true;
    }

}
