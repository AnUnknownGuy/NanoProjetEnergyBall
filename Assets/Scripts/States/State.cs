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
        //Debug.Log(name);
    }

    public virtual void Start(float param) {

    }

    public virtual void Update() {
        player.LooseHealth();
    }

    public virtual void BallEntered(Ball ball) {
        if (!ball.sleeping) {
            if (ball.charged && ball.previousPlayer != player) {
                player.stateManager.numberHittedByBall++;
                player.SetSpeed(ball.GetSpeed() * 0.2f);
				AudioManager.Ball_Hit(player.gameObject);
                ball.FakeCollision();
                player.animator.Play("hit");
                ball.Hit();
                player.looseHealthBallHit();
                player.ToStunState();
            } else if (!ball.charged) {
                if (ball.previousPlayertouched == player && ball.previousPlayertouchedTimeStamp + ball.timeBeforeballCanBeCatchBySamePlayer < Time.time) {

                    if (player.CatchBall(ball)) {
                        player.ToHoldState();
						AudioManager.Ball_Get(player.gameObject);
					}

                } else if (ball.previousPlayertouched != player) {

                    if (player.CatchBall(ball)) {
                        player.ToHoldState();
						AudioManager.Ball_Get(player.gameObject);
					}
                }
            }
            ball.previousPlayertouched = player;
            ball.previousPlayertouchedTimeStamp = Time.time;
        }
        
    }

    public string GetName() {
        return name;
    }


    public virtual void DashEntered(Player otherPlayer) {
        if (player.HasBall()) {
            AudioManager.Dash_Hit(player.gameObject);
            player.stateManager.numberHittedByDash++;

            Ball ball = player.ball;
            ball.Free();
            player.ball = null;
            ball.SetSpeedWhenFreeFromDash();


            player.ToStunState(player.hitStunDuration);
            player.SetSpeed(otherPlayer.rb.velocity * player.hitSpeedTransfert);

            otherPlayer.SetSpeed(Vector2.zero);
            otherPlayer.ToBaseState();
            player.looseHealthDashHit();
        }
    }

    public virtual void WallCollided(Vector2 collisionDirection) {
        
    }

    public virtual void PlateformCollided(GameObject plateforme) {
        player.lastPlateformTouched = plateforme;
    }
}
