using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public float inputThresholdJump = 0.3f;
    public float inputThresholdFastFall = -0.1f;
    public float inputBufferDuration = 0.2f;

    private float deadZoneRightStick = 0.2f;
    private bool canHaveActionOnRightStick = true;

    private float minimumRightStickSize = 0.05f;

    private CustomInput previousLeftStickValue;
    private Vector2 leftSpeed;

    private CustomInput previousRightStickValue;
    private Vector2 rightSpeed;

    public float stopJumpThreshhold = 0.5f;

    private float jumpStartTimeStamp = 0;
    private float jumpStopTimeStamp = 0;
    private float actionTimeStamp = 0;
    private float fastFallTimeStamp = 0;

    public PlayerInput playerInput; 
    public PlayerSettings settings;
	
    private float timerBeforeInputStart = 0.5f;
    private float timeStart;

    public Player player;

    void Start()
    {
        previousLeftStickValue = new CustomInput(Vector2.zero, 0);
        previousRightStickValue = new CustomInput(new Vector2(0, -0.1f), 0);

        leftSpeed = Vector2.zero;
        rightSpeed = Vector2.zero;
        settings = new PlayerSettings(false, AimingControls.Rightstick);
        timeStart = Time.time;
    }

    void Update()
    {
        if (Time.time > timeStart + timerBeforeInputStart) {
            player.stateManager.OnLeftStick(previousLeftStickValue.value);

            if (!OutDated(jumpStartTimeStamp)) {
                if (player.stateManager.OnJump())
                    jumpStartTimeStamp = 0;

            }
            if (!OutDated(jumpStopTimeStamp)) {
                if (player.stateManager.OnJumpStop())
                    jumpStopTimeStamp = 0;

            }
            if (!OutDated(actionTimeStamp)) {
                if (player.stateManager.OnAction())
                    actionTimeStamp = 0;

            }
            if (!OutDated(fastFallTimeStamp)) {
                if (player.stateManager.OnFastFall())
                    fastFallTimeStamp = 0;
            }
        }
    }

    public void RightStick(InputAction.CallbackContext context) {
        if (context.performed) {
            RightStickProcess(context.ReadValue<Vector2>());
        } else if(context.canceled) {
            RightStickProcess(Vector2.zero);
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
            JumpProcess();
            if (previousLeftStickValue.value.y < inputThresholdFastFall) 
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
                    GameManager.ResumeGame();
                }
                else
                {
                    pauseMenu.gameObject.SetActive(true);
                    pauseMenu.TargetPlayer = this;
                    playerInput.SwitchCurrentActionMap("UI");
                    GameManager.PauseGame();
                }
            }
            else Debug.LogError("PauseMenu not found");
        }
    }

    void RightStickProcess(Vector2 value) {
        if (settings.aimingControls == AimingControls.Rightstick) {
            if (value.magnitude >= deadZoneRightStick && canHaveActionOnRightStick) {

                ActionProcess();
                canHaveActionOnRightStick = false;
            } else if (value.magnitude < deadZoneRightStick) {
                canHaveActionOnRightStick = true;
            }
        }
        
        if (value.magnitude >= minimumRightStickSize) {
            previousRightStickValue = new CustomInput(value);
        } else if (value.magnitude != 0) {
            previousRightStickValue = new CustomInput(value.normalized / (1 / minimumRightStickSize));
        } else {
            previousRightStickValue = new CustomInput(previousRightStickValue.value.normalized / (1 / minimumRightStickSize));
        }
    }

    void LeftStickProcess(Vector2 value)
    {
        float verticalMovement = value.y;
        
        if (settings.jumpWithStick 
            && verticalMovement >= inputThresholdJump
            && previousLeftStickValue.value.magnitude < value.magnitude) {
            JumpProcess();
        }
        if (verticalMovement <= inputThresholdFastFall) {
            FastFallProcess();
        }
        if (settings.jumpWithStick && previousLeftStickValue.value.y > stopJumpThreshhold && value.y < stopJumpThreshhold) {
            JumpStop();
        }
        
        previousLeftStickValue = new CustomInput(value);
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
    public Vector2 GetRightStickValue()
    {
        return (settings.aimingControls == AimingControls.LeftstickButton)
            ? previousLeftStickValue.value
            : previousRightStickValue.value;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, (Vector2)(transform.position) + previousRightStickValue.value);
    }
}

struct CustomInput {
    public Vector2 value;
    public float timeStamp;

    public CustomInput(Vector2 v) : this(v, Time.time) {

    }

    public CustomInput(Vector2 v, float t) {
        value = v;
        timeStamp = Time.time;
    }

}

public enum AimingControls
{
    Rightstick, RightStickButton, LeftstickButton
}