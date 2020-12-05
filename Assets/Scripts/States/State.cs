using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : StateInterface
{
    protected Player player;

    protected string name = "State";

    public Color color = Color.black;

    protected State(Player player) {
        this.player = player;
    }

    public virtual void HitSignal() {
        
    }

    public virtual bool JumpSignal() {
        return false;
    }
    public virtual bool JumpStopSignal() {
        return false;
    }

    public virtual bool FastFallSignal() {
        return false;
    }

    public virtual void WalkSignal(float x) {

    }

    public virtual bool ActionSignal() {
        return false;
    }

    public virtual void Stop() {

    }

    public virtual void Start() {
        Debug.Log(name);
    }

    public virtual void Start(float param) {

    }

    public virtual void Update() {
        if (!player.onGround) player.ProcessJump();
    }

    public virtual void BallEntered(Ball ball) {

    }

    public string GetName() {
        return name;
    }

    public virtual void DashEntered(Player otherPlayer) {
        if (player.HasBall()) {
            Ball ball = player.ball;
            ball.Free();
            player.ball = null;
            otherPlayer.CatchBall(ball);
            player.ToStunState(player.hitStunDuration);
            player.SetSpeed(otherPlayer.rb.velocity * player.hitSpeedTransfert);
            otherPlayer.SetSpeed(Vector2.zero);
        }
    }

    public virtual void WallCollided(Vector2 collisionDirection) {
        
    }
}
