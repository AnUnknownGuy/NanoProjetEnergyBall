using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class Vibration_Test : MonoBehaviour
{

    PlayerIndex playerIndex;
    GamePadState state;
    GamePadState prevState;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        GamePad.SetVibration(playerIndex, 0.5f, 0.5f);
    }
}
