using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : StateInterface
{
    protected Player player;
    protected string name = "State";
    public Color color = Color.black;
    protected AudioManager audioManager;

    protected State(Player player, AudioManager audioManager) {
        this.player = player;
        this.audioManager = audioManager;
        Debug.Log(audioManager == null);
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
        //Debug.Log(name);
    }

    public virtual void Start(float param) {

    }

    public virtual void Update() {
    }

    public virtual void BallEntered(Ball ball) {
        if (ball.charged && ball.previousPlayer != player) {
            audioManager.Ball_hit.Post(player.gameObject);
            player.stateManager.numberHittedByBall++;
            player.SetSpeed(ball.GetSpeed() * 0.2f);
            ball.FakeCollision();
            ball.Hit();
            player.ToStunState();
        }
    }

    public string GetName() {
        return name;
    }

    public virtual void DashEntered(Player otherPlayer) {
        if (player.HasBall()) {
            audioManager.Dash_hit.Post(player.gameObject);
            player.stateManager.numberHittedByDash++;
            Ball ball = player.ball;
            ball.Free();
            player.ball = null;
            ball.SetSpeed(Vector2.up);
            otherPlayer.ToBaseState();
            otherPlayer.SetSpeed(Vector2.zero);
            player.ToStunState(player.hitStunDuration);
            player.SetSpeed(otherPlayer.rb.velocity * player.hitSpeedTransfert);
        }
    }

    public virtual void WallCollided(Vector2 collisionDirection) {
        
    }
}
