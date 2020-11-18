using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StateManager 
{
    public float stunDuration = 0.4f;

    private Player player;

    private State currentState;

    private StunState stunState;
    private HoldState holdState;
    private BaseState baseState;
    
    public StateManager(Player player) {
        this.player = player;

        stunState = new StunState(player, stunDuration);
        holdState = new HoldState(player);
        baseState = new BaseState(player);

        currentState = baseState;
    }

    private void ToNewState(State state) {
        state.Stop();
        currentState = state;
        currentState.Start();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        
        if (direction.y >= player.inputThreshold) currentState.JumpSignal();
        else if (direction.y <= -player.inputThreshold) currentState.FastFallSignal();
        
        currentState.WalkSignal(direction.x);
    }

    public void OnRightStick(InputAction.CallbackContext context)
    {
        currentState.RightstickSignal(context.ReadValue<Vector2>());
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

    public void SendBallEntered(Ball ball) {
        currentState.BallEntered(ball);
    }

    public void Update() {
        currentState.Update();
    }
}
