
public class PlayerSettings
{
    public bool jumpWithStick;
    public AimingControls aimingControls;

    public PlayerSettings(bool _jumpWithStick, AimingControls _aimingControls)
    {
        jumpWithStick = _jumpWithStick;
        aimingControls = _aimingControls;
    }
}
