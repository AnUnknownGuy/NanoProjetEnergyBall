﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManager 
{
    public float stunDuration = 0.4f;
    public float dashDuration = 0.4f;

    private Player player;

    public State currentState;

    private StunState stunState;
    private HoldState holdState;
    private BaseState baseState;
    private DashState dashState;

    public StateManager(Player player) {
        this.player = player;

        stunState = new StunState(player, stunDuration);
        holdState = new HoldState(player);
        baseState = new BaseState(player);
        dashState = new DashState(player, dashDuration);

        currentState = baseState;
    }

    private void ToNewState(State state) {
        state.Stop();
        currentState = state;
        currentState.Start();
    }

    public void OnLeftStick(Vector2 dir)
    {
        currentState.WalkSignal(dir.x);
    }

    public bool OnJump() {
        return currentState.JumpSignal();
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

    public void Update() {
        currentState.Update();
    }
}