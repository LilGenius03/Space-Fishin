using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerController : MonoBehaviour, InputSystem_Actions.IPlayerActions
{
    public delegate void Event_OnFreezeInput();
    public static event Event_OnFreezeInput DE_OnFreezeInput;
    public delegate void Event_OnUnFreezeInput();
    public static event Event_OnUnFreezeInput DE_OnUnFreezeInput;

    InputSystem_Actions playerControls;

    PlayerMovement player_movement;
    PlayerInteraction player_interaction;

    public FishingRod fishingRod;
    FishMovement fishMovement;

    public Camera cam;
    public GameObject bob;
    Coroutine castCoroutine;
    public bool isCast;
    public bool bobActive = false;
    public GameObject bobArea;

    public float lineDistance;
    public float lineSpeed;
    public float bobSpeed;
    [SerializeField] LayerMask fishingLayer;
    public float reelSpeed;
    public bool inputActive;

    bool is_move_inputing, is_look_inputing;
    Vector2 move_input, look_input;

    void Awake()
    {
        playerControls = new InputSystem_Actions();
        playerControls.Player.SetCallbacks(this);
        playerControls.Enable();
        player_movement = GetComponent<PlayerMovement>();
        player_interaction = GetComponent<PlayerInteraction>();
        Cursor.visible = false;
    }

    void Update()
    {
        player_movement.SetLookInput(look_input);

        player_movement.SetMovementInput(move_input);
    }

    void FixedUpdate()
    {
        if (!isCast && !bobActive)
        {
            Vector3 LerpedPosition = Vector3.Lerp(bob.transform.position, bobArea.transform.position, Time.fixedDeltaTime * bobSpeed);
            bob.transform.position = new Vector3(LerpedPosition.x, bobArea.transform.position.y, LerpedPosition.z);
        }
    }

    public void FreezeInput()
    {
        DE_OnFreezeInput?.Invoke();
        playerControls.Disable();
    }

    public void UnFreezeInput()
    {
        DE_OnUnFreezeInput?.Invoke();
        playerControls.Enable();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            inputActive = true;
            if (isCast == false)
            {
                isCast = true;
                bobActive = true;
                if (castCoroutine != null)
                {
                    StopCoroutine(castCoroutine);
                }
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, lineDistance, fishingLayer))
                {
                    //bob.transform.position = hit.point;
                    castCoroutine = StartCoroutine(CastTravel(hit.point));
                    if (hit.transform.CompareTag("FishingArea"))
                    {
                        fishMovement = hit.transform.GetComponent<FishMovement>();
                        fishMovement.isHooked = true;
                        //bob.transform.position = hit.transform.position;
                        Debug.Log("Fishin");
                    }
                }
                else
                {
                    //bob.transform.position = cam.transform.position + (cam.transform.forward * lineDistance);
                    castCoroutine = StartCoroutine(CastTravel(cam.transform.position + (cam.transform.forward * lineDistance)));
                }
            }
            else
            {
                StopCoroutine(castCoroutine);
                castCoroutine = StartCoroutine(ReturnCast());
                isCast = false;
            }
        }
        else
            inputActive = false;
    }

    public IEnumerator CastTravel(Vector3 castPosition)
    {
        while(Vector3.Distance(bob.transform.position, castPosition) > 0)
        {
            //bob.transform.position += cam.transform.forward * Time.deltaTime;
            bob.transform.position = Vector3.Lerp(bob.transform.position, castPosition, (lineSpeed * Time.deltaTime));
            yield return null;
        }
        bob.transform.position = castPosition;
    }

    public IEnumerator ReturnCast()
    {
        while (Vector3.Distance(bob.transform.position, bobArea.transform.position) > 1)
        {
            //bob.transform.position += -cam.transform.forward * Time.deltaTime;
            bob.transform.position = Vector3.Lerp(bob.transform.position, bobArea.transform.position, (lineSpeed * Time.deltaTime));
            yield return null;
        }
        bob.transform.position = bobArea.transform.position;
        bobActive = false;
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
