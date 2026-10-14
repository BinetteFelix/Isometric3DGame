using Utility;

public class InputManager : SingletonBehaviour<InputManager>
{
    public InputSystem_Actions actions;

    private void Start()
    {
        actions.Player.Move.Enable();
        actions.Player.Look.Enable();
        actions.Player.LookController.Enable();
        actions.UI.Pausing.Enable();
        actions.UI.PressNorth.Enable();
        actions.UI.PressSouth.Enable();
        actions.UI.PressEast.Enable();
        actions.UI.PressWest.Enable();
    }
    public override void Instantiate()
    {
        actions = new InputSystem_Actions();
    }
}
