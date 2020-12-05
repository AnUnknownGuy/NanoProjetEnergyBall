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
    public bool actionOnSticks = true;

    public float timeBetweenStickUpdate = 0.1f;

    private float minimumRightStickSize = 0.05f;

    [HideInInspector] private Input previousLeftStickValue;
    [HideInInspector] private Vector2 leftSpeed;

    [HideInInspector] private Input previousRightStickValue;
    [HideInInspector] private Vector2 rightSpeed;

    public float stopJumpThreshhold = 0.5f;

    private float jumpStartTimeStamp = 0;
    private float jumpStopTimeStamp = 0;
    private float actionTimeStamp = 0;
    private float fastFallTimeStamp = 0;

    public Player player;

    // Start is called before the first frame update
    void Start()
    {
        previousLeftStickValue = new Input(Vector2.zero, 0);
        previousRightStickValue = new Input(new Vector2(0, -0.1f), 0);

        leftSpeed = Vector2.zero;
        rightSpeed = Vector2.zero;

        BindControls();
    }

    // Update is called once per frame
    void Update()
    {
        player.stateManager.OnLeftStick(previousLeftStickValue.value);

        /*
        if (previousLeftStickValue.timeStamp + timeBetweenStickUpdate <= Time.time) {
            LeftStick();
        }

        if (previousRightStickValue.timeStamp + timeBetweenStickUpdate <= Time.time) {
            RightStick();
        }
        */

        if (!OutDated(jumpStartTimeStamp) ) {
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

    void BindControls() {

        /*
        playerControls.Controls.Move.performed += LeftStick;
        playerControls.Controls.Rightstick.performed += RightStick;
        playerControls.Controls.Jump.started += LeftShoulder;
        playerControls.Controls.Action.started += RightShoulder;

        playerControls.Controls.Move.Enable();
        playerControls.Controls.Rightstick.Enable();
        playerControls.Controls.Jump.Enable();
        playerControls.Controls.Action.Enable();
        */
    }

    /*
    void RightStick() {
        Vector2 value = playerControls.Controls.Rightstick.ReadValue<Vector2>();

        RightStickProcess(value);
    }
    */

    public void RightStick(InputAction.CallbackContext context) {

        if (context.performed) {
            Vector2 value = context.ReadValue<Vector2>();

            RightStickProcess(value);
        }
    }

    void RightStickProcess(Vector2 value) {

        if (actionOnSticks)
            //si la différence est assez grande et que le stick ne revient pas vers la position position 0,0
            if ((value - previousRightStickValue.value).magnitude >= inputSpeedThresholdAction && previousRightStickValue.value.magnitude < value.magnitude) {
                RightShoulder();
            }

        if (value.magnitude >= minimumRightStickSize) {
            previousRightStickValue = new Input(value);
        } else if (value.magnitude != 0) {
            previousRightStickValue = new Input(value.normalized / (1 / minimumRightStickSize));
        } else {
            previousRightStickValue = new Input(previousRightStickValue.value.normalized / (1 / minimumRightStickSize));
        }

    }

    /*
    void LeftStick() {
        Vector2 value = playerControls.Controls.Move.ReadValue<Vector2>();
        LeftStickProcess(value);
    }
    */

    public void LeftStick(InputAction.CallbackContext context) {
        if (context.performed) {
            LeftStickProcess(context.ReadValue<Vector2>());
        } else if (context.canceled) {
            LeftStickProcess(Vector2.zero);
        }
    }

    void LeftStickProcess(Vector2 value) {
        if (actionOnSticks)
            //si la différence est assez grande et que le stick ne revient pas vers la position position 0,0
            if (value.y - previousLeftStickValue.value.y /*/ (Time.time - previousLeftStickValue.timeStamp)*/ >= inputSpeedThresholdJump && previousLeftStickValue.value.magnitude < value.magnitude) {
                LeftShoulder();
            }

        if ((value.y - previousLeftStickValue.value.y) /*/ (Time.time - previousLeftStickValue.timeStamp)*/ <= -inputSpeedThresholdFastFall && previousLeftStickValue.value.magnitude < value.magnitude) {
            FastFall();
        }

        if (previousLeftStickValue.value.y > stopJumpThreshhold && value.y < stopJumpThreshhold) {
            JumpStop();
        }

        previousLeftStickValue = new Input(value);
    }

    public void RightShoulder(InputAction.CallbackContext context) {
        if (context.performed) {
            RightShoulderProcess();
        }
    }
    void RightShoulder() {
        RightShoulderProcess();
    }

    void RightShoulderProcess() {
        actionTimeStamp = Time.time;
    }

    void LeftShoulder() {
        LeftShoulderProcess();
    }

    public void LeftShoulder(InputAction.CallbackContext context) {
        if (context.performed) {
            LeftShoulderProcess();
        } else if (context.canceled) {
            JumpStop();
        }
    }

    void LeftShoulderProcess() {
        jumpStartTimeStamp = Time.time;
    }

    public void FastFall(InputAction.CallbackContext context) {
        if (context.performed) {
            FastFallProcess();
        }
    }

    void FastFall() {
        FastFallProcess();
    }

    void FastFallProcess() {
        fastFallTimeStamp = Time.time;
    }

    void JumpStop() {
        jumpStopTimeStamp = Time.time;
    }

    public bool GetFastFall() {
        return !OutDated(fastFallTimeStamp);
    }

    public Vector2 GetLeftStickValue() {
        return previousLeftStickValue.value;
    }

    /*
    public Vector2 GetRightStickValue() {

        Vector2 actualPosition = playerControls.Controls.Rightstick.ReadValue<Vector2>();

        if (actualPosition.magnitude >= 0.05f) {
            return actualPosition;
        } else {
            return previousRightStickValue.value;
        }
    }*/

    public Vector2 GetRightStickValue() {
        return previousRightStickValue.value;
    }

    /*
    public void Stop() {
        playerControls.Disable();
    }
    */

    public bool OutDated(float timeStamp) {
        return timeStamp + inputBufferDuration < Time.time;
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