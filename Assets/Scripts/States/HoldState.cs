using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldState : MoveState
{

    public HoldState(Player player): base(player) {
        name = "HoldState";
    }

    public override void RightstickSignal(Vector2 v) {
        base.RightstickSignal(v);
        if (v.magnitude >= player.inputThreshold) {
            player.ThrowBall();
            player.ToStunState();//à changer, pour eviter d'attrapper la balle tout de suite
        }
    }

}
