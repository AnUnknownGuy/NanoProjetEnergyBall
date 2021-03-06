﻿using UnityEngine;

public class TemporaryState : State
{

    protected float endingTime;
    protected float timeInState;

    protected TemporaryState(Player player, float time) : base(player) {
        name = "TemporaryState";
        this.timeInState = time;
    }

    override public void Start() {
        base.Start();
        endingTime = Time.time + timeInState;
    }

    override public void Start(float param) {
        endingTime = Time.time + param;
    }

    override public void Update() {
        base.Update();
        if (Time.time >= endingTime) {
            NextState();
        }
    }

    public virtual void NextState() {

    }
}
