using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MoveState {

    private float lastDashTimestamp = 0;
    

    public BaseState(Player player) : base(player) {
        name = "BaseState";
        color = Color.green;
    }

    override public void Update() {
        base.Update();
    }

    public override bool ActionSignal() {
        if (player.canDash && lastDashTimestamp + player.timeBetweenDash < Time.time) {
            player.ToDashState();

            AudioManager.Dash(player.gameObject);
            lastDashTimestamp = Time.time;
            return true;
        } else {
            return false;
        }
    }
}
