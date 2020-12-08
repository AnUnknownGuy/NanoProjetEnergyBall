using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldStunState : StunState {


    public HoldStunState(Player player, AudioManager audioManager, float stunTime) : base(player, audioManager, stunTime) {
        name = "HoldStunState";
        color = Color.red;
    }

    override public void NextState() {
        player.ToHoldState();
    }
}