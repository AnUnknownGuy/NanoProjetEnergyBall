using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public float inputSpeedThresholdAction = 0.3f;
    public float inputSpeedThresholdJump = 0.3f;
    public float inputSpeedThresholdFastFall = 0.2f;
    public float inputBufferDuration = 0.2f;

    public float timeBetweenStickUpdate = 0.1f;

    private float minimumRightStickSize = 0.05f;

    private Input previousLeftStickValue;
    private Vector2 leftSpeed;

    private Input previousRightStickValue;
    private Vector2 rightSpeed;

    public float stopJumpThreshhold = 0.5f;

    private float jumpStartTimeStamp = 0;
    private float jumpStopTimeStamp = 0;
    private float actionTimeStamp = 0;
    private float fastFallTimeStamp = 0;

    public PlayerInput playerInput; 
    public PlayerSettings settings;
    public Player player;

    void Start()
    {
        previousLeftStickValue = new Input(Vector2.zero, 0);
        previousRightStickValue = new Input(new Vector2(0, -0.1f), 0);

        leftSpeed = Vector2.zero;
        rightSpeed = Vector2.zero;
        
        settings = new PlayerSettings(false, AimingControls.Rightstick);
    }

    void Update()
    {
        player.stateManager.OnLeftStick(previousLeftStickValue.value);
        
        if (!OutDated(jumpStartTimeStamp) && player.stateManager.OnJump())
                jumpStartTimeStamp = 0;
        
        if (!OutDated(jumpStopTimeStamp) && player.stateManager.OnJumpStop())
                jumpStopTimeStamp = 0;
        
        if (!OutDated(actionTimeStamp) && player.stateManager.OnAction())
                actionTimeStamp = 0;
        
        if (!OutDated(fastFallTimeStamp) && player.stateManager.OnFastFall())
                fastFallTimeStamp = 0;
    }

    public void RightStick(InputAction.CallbackContext context) {
        if (context.performed) {
            RightStickProcess(context.ReadValue<Vector2>());
        }
    }

    public void LeftStick(InputAction.CallbackContext context) {
        if (context.performed) {
            LeftStickProcess(context.ReadValue<Vector2>());
        } else if (context.canceled) {
            LeftStickProcess(Vector2.zero);
        }
    }

    public void RightShoulder(InputAction.CallbackContext context) {
        if (context.performed && settings.aimingControls != AimingControls.Rightstick) {
            ActionProcess();
        }
    }

    public void LeftShoulder(InputAction.CallbackContext context) {
        if (context.performed && !settings.jumpWithStick) {
            if (previousLeftStickValue.value.y > 0) 
                JumpProcess();
            else 
                FastFallProcess();
        } else if (context.canceled) {
            JumpStop();
        }
    }

    public void FastFall(InputAction.CallbackContext context) {
        if (context.performed) {
            FastFallProcess();
        }
    }
    
    public void ToggleMenu(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseMenu pauseMenu = PauseMenu.Instance;
            if (pauseMenu != null)
            {
                if (pauseMenu.gameObject.activeSelf)
                {
                    pauseMenu.gameObject.SetActive(false);
                    playerInput.SwitchCurrentActionMap("Controls");
                }
                else
                {
                    pauseMenu.gameObject.SetActive(true);
                    pauseMenu.TargetPlayer = this;
                    playerInput.SwitchCurrentActionMap("UI");
                }
            }
            else Debug.LogError("PauseMenu not found");
        }
    }

    void RightStickProcess(Vector2 value) {
        if (settings.aimingControls == AimingControls.Rightstick
            && (value - previousRightStickValue.value).magnitude >= inputSpeedThresholdAction 
            && previousRightStickValue.value.magnitude < value.magnitude)
            ActionProcess();
        
        if (value.magnitude >= minimumRightStickSize) {
            previousRightStickValue = new Input(value);
        } else if (value.magnitude != 0) {
            previousRightStickValue = new Input(value.normalized / (1 / minimumRightStickSize));
        } else {
            previousRightStickValue = new Input(previousRightStickValue.value.normalized / (1 / minimumRightStickSize));
        }
    }

    void LeftStickProcess(Vector2 value)
    {
        if (settings.jumpWithStick && previousLeftStickValue.value.magnitude < value.magnitude)
        {
            float verticalMovement = value.y - previousLeftStickValue.value.y;
            
            if (verticalMovement >= inputSpeedThresholdJump)
                JumpProcess();
            if (verticalMovement <= -inputSpeedThresholdFastFall)
                FastFallProcess();
        }
        if (previousLeftStickValue.value.y > stopJumpThreshhold && value.y < stopJumpThreshhold)
            JumpStop();
        
        previousLeftStickValue = new Input(value);
    }

    void ActionProcess() {
        actionTimeStamp = Time.time;
    }

    void JumpProcess() {
        jumpStartTimeStamp = Time.time;
    }

    void FastFallProcess() {
        fastFallTimeStamp = Time.time;
    }

    void JumpStop() {
        jumpStopTimeStamp = Time.time;
    }

    public bool OutDated(float timeStamp) {
        return timeStamp + inputBufferDuration < Time.time;
    }

    public bool GetFastFall() {
        return !OutDated(fastFallTimeStamp);
    }

    public Vector2 GetLeftStickValue() {
        return previousLeftStickValue.value;
    }
    public Vector2 GetRightStickValue() {
        return previousRightStickValue.value;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)(transform.position) + previousRightStickValue.value);
    }
}

struct Input {
    public Vector2 value;
    public float timeStamp;

    public Input(Vector2 v) : this(v, Time.time) {

    }

    public Input(Vector2 v, float t) {
        value = v;
        timeStamp = Time.time;
    }

}

public enum AimingControls
{
    Rightstick, RightStickButton, LeftstickButton
}