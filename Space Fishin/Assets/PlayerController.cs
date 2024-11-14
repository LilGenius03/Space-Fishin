using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    InputSystem_Actions playerControls;

    PlayerMovement player_movement;

    void Awake()
    {
        playerControls = new InputSystem_Actions();
        playerControls.Player.SetCallbacks(this);
        playerControls.Enable();
        player_movement = GetComponent<PlayerMovement>();
        Cursor.visible = false;
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            player_movement.DoCrouch();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        player_movement.Jump();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        player_movement.SetLookInput(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        player_movement.SetMovementInput(context.ReadValue<Vector2>());
    }

    public void OnNext(InputAction.CallbackContext context)
    {
        
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
        
    }

    public void OnAscend(InputAction.CallbackContext context)
    {
        if (context.started)
            player_movement.SetAscend(true);
        if(context.canceled)
            player_movement.SetAscend(false);
    }

    public void OnDescend(InputAction.CallbackContext context)
    {
        if (context.started)
            player_movement.SetDescend(true);
        if (context.canceled)
            player_movement.SetDescend(false);
    }
}
