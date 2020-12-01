using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public float inputSpeedThresholdAction = 0.3f;
    public float inputSpeedThresholdJump = 0.3f;
    public float inputSpeedThresholdFastFall = 0.3f;
    public float inputBufferDuration = 0.2f;
    public bool actionOnSticks = true;

    public float timeBetweenStickUpdate = 0.1f;

    private float minimumRightStickSize = 0.05f;

    [HideInInspector] private Input previousLeftStickValue;
    [HideInInspector] private Input previousRightStickValue;


    private float jumpTimeStamp = 0;
    private float actionTimeStamp = 0;
    private float fastFallTimeStamp = 0;

    public Player player;

    private PlayerControls playerControls;
    // Start is called before the first frame update
    void Start()
    {
        previousLeftStickValue = new Input(Vector2.zero, 0);
        previousRightStickValue = new Input(Vector2.zero, 0);

        BindControls();
    }

    // Update is called once per frame
    void Update()
    {
        player.stateManager.OnLeftStick(playerControls.Controls.Move.ReadValue<Vector2>());

        if (previousLeftStickValue.timeStamp + timeBetweenStickUpdate <= Time.time) {
            LeftStick();
        }

        if (previousRightStickValue.timeStamp + timeBetweenStickUpdate <= Time.time) { 
            RightStick();
        }

        if (!OutDated(jumpTimeStamp) ) {
            if (player.stateManager.OnJump())
                jumpTimeStamp = 0;

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
        playerControls = new PlayerControls();

        playerControls.Controls.Move.performed += LeftStick;
        playerControls.Controls.Rightstick.performed += RightStick;
        playerControls.Controls.Jump.started += LeftShoulder;
        playerControls.Controls.Action.started += RightShoulder;

        playerControls.Controls.Move.Enable();
        playerControls.Controls.Rightstick.Enable();
        playerControls.Controls.Jump.Enable();
        playerControls.Controls.Action.Enable();
    }

    void RightStick() {
        Vector2 value = playerControls.Controls.Rightstick.ReadValue<Vector2>();

        RightStickProcess(value);
    }
    void RightStick(InputAction.CallbackContext context) {
        Vector2 value = context.ReadValue<Vector2>();

        RightStickProcess(value);
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

    void LeftStick() {
        Vector2 value = playerControls.Controls.Move.ReadValue<Vector2>();
        LeftStickProcess(value);
    }

    void LeftStick(InputAction.CallbackContext context) {
        Vector2 value = context.ReadValue<Vector2>();
        LeftStickProcess(value);
    }

    void LeftStickProcess(Vector2 value) {
        if (actionOnSticks)
            //si la différence est assez grande et que le stick ne revient pas vers la position position 0,0
            if ((value.y - previousLeftStickValue.value.y) >= inputSpeedThresholdJump && previousLeftStickValue.value.magnitude < value.magnitude) {
                LeftShoulder();
            }

        if ((value.y - previousLeftStickValue.value.y) <= -inputSpeedThresholdFastFall && previousLeftStickValue.value.magnitude < value.magnitude) {
            FastFall();
        }

        previousLeftStickValue = new Input(value);
    }

    void RightShoulder(InputAction.CallbackContext context) {
        RightShoulderProcess();
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

    void LeftShoulder(InputAction.CallbackContext context) {
        LeftShoulderProcess();
    }

    void LeftShoulderProcess() {
        jumpTimeStamp = Time.time;
    }

    void FastFall(InputAction.CallbackContext context) {
        FastFallProcess();
    }

    void FastFall() {
        FastFallProcess();
    }

    void FastFallProcess() {
        fastFallTimeStamp = Time.time;
    }


    public bool GetFastFall() {
        return !OutDated(fastFallTimeStamp);
    }

    public Vector2 GetLeftStickValue() {
        return previousLeftStickValue.value;
    }

    public Vector2 GetRightStickValue() {

        Vector2 actualPosition = playerControls.Controls.Rightstick.ReadValue<Vector2>();

        if (actualPosition.magnitude >= 0.05f) {
            return actualPosition;
        } else {
            return previousRightStickValue.value;
        }

    }

    public void Stop() {
        playerControls.Disable();
    }

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