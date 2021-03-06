﻿using UnityEngine;

public class StateManager 
{
    public float stunDuration = 0.4f;

    private Player player;

    public State currentState;

    public StunState stunState;
    public HoldState holdState;
    private HoldStunState holdStunState;
    public BaseState baseState;
    public DashState dashState;

    //Log
    public int numberHittedByDash = 0;
    public int numberHittedByBall = 0;
    public int numberOfJumps = 0;
    public int numberOfFastFall = 0;
    public int numberOfDash = 0;
    public float timeInDash = 0;
    public float timeInHold = 0;
    public int numberOfBallCatched = 0;

    public StateManager(Player player) {
        this.player = player;

        stunState = new StunState(player, stunDuration);
        holdState = new HoldState(player);
        holdStunState = new HoldStunState(player, stunDuration);
        baseState = new BaseState(player);
        dashState = new DashState(player, player.dashDuration);

        currentState = baseState;
    }

    private void ToNewState(State state) {
        currentState.Stop();
        currentState = state;
        currentState.Start();
    }

    private void ToNewState(State state, float param) {
        currentState.Stop();
        currentState = state;
        currentState.Start(param);
    }

    public void OnLeftStick(Vector2 dir)
    {
        currentState.WalkSignal(dir.x);
    }

    public bool OnJump() {
        return currentState.JumpSignal();
    }

    public bool OnJumpStop() {
        return currentState.JumpStopSignal();
    }
    public bool OnFastFall() {
        return currentState.FastFallSignal();
    }

    public bool OnAction()
    {
        return currentState.ActionSignal();
    }
    
    public void ToStun() {
        ToNewState(stunState);
    }

    public void ToStun(float duration) {
        ToNewState(stunState, duration);
    }
    public void ToHoldStun(float duration) {
        ToNewState(holdStunState, duration);
    }

    public void ToBase() {
        ToNewState(baseState);
    }
    public void ToHold() {
        ToNewState(holdState);
    }
    public void ToDash() {
        ToNewState(dashState);
    }

    public void OnBallEntered(Ball ball) {
        currentState.BallEntered(ball);
    }

    public void OnGroundTouched() {
        currentState.GroundTouched();
    }

    public void OnDashEntered(Player otherPlayer) {
        currentState.DashEntered(otherPlayer);
    }

    public void OnWallCollided(Vector2 collisionDirection) {
        currentState.WallCollided(collisionDirection);
    }

    public void OnPlateformCollided(GameObject plateforme) {
        currentState.PlateformCollided(plateforme);
    }


    public void Update() {
        currentState.Update();
    }
}
