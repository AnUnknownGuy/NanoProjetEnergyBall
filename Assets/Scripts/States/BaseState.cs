using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MoveState {

    private float lastDashTimestamp = 0;
    

    public BaseState(Player player, AudioManager audioManager) : base(player, audioManager) {
        name = "BaseState";
        color = Color.green;
    }

    override public void Update() {
        base.Update();
        player.LooseHealth();
    }

    public override bool ActionSignal() {
        if (player.canDash && lastDashTimestamp + player.timeBetweenDash < Time.time) {
            player.ToDashState();
            
            audioManager.Dash.Post(player.gameObject);
            lastDashTimestamp = Time.time;
            return true;
        } else {
            return false;
        }
    }
}
