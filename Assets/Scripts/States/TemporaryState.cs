using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporaryState : State
{

    protected float endingTime;
    protected float timeInState;

    protected TemporaryState(Player player, float time) : base(player) {
        name = "TemporaryState";
        this.timeInState = time;
    }

    override public void Start() {
        endingTime = Time.time + timeInState;
    }

    override public void Update() {
        if (Time.time >= endingTime) {
            NextState();
        }
    }

    public virtual void NextState() {

    }
}
