using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : StateInterface
{
    protected Player player;

    protected string name = "State";

    protected State(Player player) {
        this.player = player;
    }

    public virtual void HitSignal() {
        
    }

    public virtual void JumpSignal() {
        
    }

    public virtual void WalkSignal() {

    }

    public virtual void Stop() {

    }

    public virtual void Start() {

    }

    public virtual void Update() {
    }

    public virtual void BallEntered(Ball ball) {

    }

    public string GetName() {
        return name;
    }
}
