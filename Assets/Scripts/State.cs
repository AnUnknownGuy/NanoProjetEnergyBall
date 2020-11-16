using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State : StateInterface
{

    protected Player player;

    new protected string name = "State";

    protected State(Player player) {
        this.player = player;
    }

    public virtual void HitSignal() {
        
    }

    public virtual void JumpSignal() {
        
    }

    public virtual void WalkSignal() {

    }

    public virtual void Reset() {

    }

    public virtual void Start() {

    }

    public string GetName() {
        return name;
    }
}
