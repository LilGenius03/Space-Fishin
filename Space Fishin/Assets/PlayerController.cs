using NUnit;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;


public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public delegate void Event_OnFreezeInput();
    public static event Event_OnFreezeInput DE_OnFreezeInput;
    public delegate void Event_OnUnFreezeInput();
    public static event Event_OnUnFreezeInput DE_OnUnFreezeInput;

    InputSystem_Actions playerControls;

    PlayerMovement player_movement;
    PlayerInteraction player_interaction;
    PlayerFishing player_fishing;

    bool is_move_inputing, is_look_inputing;
    Vector2 move_input, look_input;

    [SerializeField] HotBarUI hobbar;

    void Awake()
    {
        playerControls = new InputSystem_Actions();
        playerControls.Player.SetCallbacks(this);
        playerControls.Enable();
        player_movement = GetComponent<PlayerMovement>();
        player_interaction = GetComponent<PlayerInteraction>();
        player_fishing = GetComponent<PlayerFishing>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        //player_movement.SetLookInput(look_input);

        player_movement.SetMovementInput(move_input);
    }

    public void FreezeInput()
    {
        DE_OnFreezeInput?.Invoke();
        playerControls.Disable();
        player_movement.freezsere = true;
    }

    public void UnFreezeInput()
    {
        DE_OnUnFreezeInput?.Invoke();
        playerControls.Enable();
        player_movement.freezsere = false;

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
            player_fishing.CastRod();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
            player_movement.DoCrouch();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("tried to int");
            player_interaction.Interact();
        }
            
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

            //look_input = context.ReadValue<Vector2>();
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

    public void OnTab(InputAction.CallbackContext context)
    {
        if (context.performed)
            hobbar.ShowUI();
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

    public void OnRoll(InputAction.CallbackContext context)
    {
        if (context.started)
            player_movement.SetRolling(true);
        if (context.canceled)
            player_movement.SetRolling(false);
    }

    public void OnSwitchBaitUp(InputAction.CallbackContext context)
    {
       // if (context.performed)
         //   player_fishing.SwitchBait(true);
    }

    public void OnSwitchBaitDown(InputAction.CallbackContext context)
    {
        //if (context.performed)
           // player_fishing.SwitchBait(false);
    }

    public void OnRelease(InputAction.CallbackContext context)
    {
        if (context.performed)
            player_fishing.Release();
    }
}
