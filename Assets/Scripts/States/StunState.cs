using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunState : TemporaryState {


    public StunState(Player player, AudioManager audioManager, float stunTime) : base(player, audioManager, stunTime) {
        name = "StunState";
        color = Color.red;
    }

    override public void NextState() {
        player.ToBaseState();
    }
}
