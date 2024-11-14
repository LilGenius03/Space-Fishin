using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    InputSystem_Actions playerControls;

    PlayerMovement player_movement;

    bool is_move_inputing, is_look_inputing;
    Vector2 move_input, look_input;

    void Awake()
    {
        playerControls = new InputSystem_Actions();
        playerControls.Player.SetCallbacks(this);
        playerControls.Enable();
        player_movement = GetComponent<PlayerMovement>();
        Cursor.visible = false;
    }

    void Update()
    {
            player_movement.SetLookInput(look_input);

            player_movement.SetMovementInput(move_input);
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
        if (context.started)
            is_look_inputing = true;

        if (context.canceled)
            is_look_inputing = false;

            look_input = context.ReadValue<Vector2>();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.started)
            is_move_inputing = true;

        if (context.canceled)
            is_move_inputing = false;

            move_input = context.ReadValue<Vector2>();
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
