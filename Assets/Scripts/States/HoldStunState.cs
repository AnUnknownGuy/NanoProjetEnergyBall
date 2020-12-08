using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldStunState : StunState {


    public HoldStunState(Player player, float stunTime) : base(player, stunTime) {
        name = "HoldStunState";
        color = Color.red;
    }

    override public void NextState() {
        player.ToHoldState();
    }
}