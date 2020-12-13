using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using XInputDotNetPure;

public class Vibration : MonoBehaviour
{
    public static void Vibrate(PlayerInput playerInput, float intensity, float duration)
    {
        Vibrate(playerInput, intensity, intensity, duration);
    }
    
    public static void Vibrate(PlayerInput playerInput, float intensityL, float intensityR, float duration)
    {
        // PlayerIndex playerIndex = (PlayerIndex) playerInput.playerIndex;
        // GamePad.SetVibration(playerIndex, intensityL, intensityR);
        // playerInput.StartCoroutine(StopVibration(playerIndex, duration));
    }

    private static IEnumerator StopVibration(PlayerIndex playerIndex, float duration)
    {
        // yield return new WaitForSeconds(duration);
        // GamePad.SetVibration(playerIndex, 0, 0);
        yield return null;
    }
}
