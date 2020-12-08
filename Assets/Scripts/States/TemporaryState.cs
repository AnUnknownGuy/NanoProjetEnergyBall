using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryState : State
{

    protected float endingTime;
    protected float timeInState;

    protected TemporaryState(Player player, AudioManager audioManager, float time) : base(player, audioManager) {
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
