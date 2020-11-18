using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : TemporaryState
{
    public DashState(Player player, float stunTime) : base(player, stunTime) {
        name = "DashState";
    }

    override public void NextState() {
        player.ToBaseState();
    }
}
