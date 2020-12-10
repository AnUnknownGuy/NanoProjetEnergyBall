using UnityEngine;

public class StunState : TemporaryState {


    public StunState(Player player, float stunTime) : base(player, stunTime) {
        name = "StunState";
        color = Color.red;
    }

    override public void NextState() {
        player.ToBaseState();
    }
    public override void BallEntered(Ball ball) {

    }
}
