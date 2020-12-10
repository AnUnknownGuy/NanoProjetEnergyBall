using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Toggle joystickJumpToggle; 
    public Dropdown aimingControlsDropdown;
    private Selectable selectedItem;
    
    private InputManager targetPlayer;
    public InputManager TargetPlayer
    {
        get { return targetPlayer; }
        set
        {
            targetPlayer = value;
            joystickJumpToggle.isOn = targetPlayer.settings.jumpWithStick;
            aimingControlsDropdown.value = (int)targetPlayer.settings.aimingControls;
            joystickJumpToggle.Select();
            selectedItem = joystickJumpToggle;
        }
    }

    public static PauseMenu Instance;

    private void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        
        BindEvents();
        gameObject.SetActive(false);
    }

    private void BindEvents()
    {
        joystickJumpToggle.onValueChanged.AddListener(
            newSetting => targetPlayer.settings.jumpWithStick = newSetting);
        aimingControlsDropdown.onValueChanged.AddListener(
            newSetting => targetPlayer.settings.aimingControls = (AimingControls) newSetting);
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        /*
        Selectable newSelection = selectedItem.FindSelectable(context.ReadValue<Vector2>());
        // Deselect selectedItem ?
        selectedItem = newSelection;
        selectedItem.Select();*/
    }

    public void Select(InputAction.CallbackContext context)
    {
        
    }
}
