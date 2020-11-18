using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : TemporaryState {


    public StunState(Player player, float stunTime) : base(player, stunTime) {
        name = "StunState";
    }

    override public void NextState() {
        player.ToBaseState();
    }
}
