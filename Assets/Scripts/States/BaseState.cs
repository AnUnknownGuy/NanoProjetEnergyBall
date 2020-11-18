using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MoveState {


    public BaseState(Player player) : base(player) {
        name = "BaseState";
    }


    override public void Update() {
        player.LooseHealth();
    }

}
