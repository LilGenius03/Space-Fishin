using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] PlayerController player;
    PlayerMovement player_movement;

    Inventory inventory;

    [SerializeField] Transform player_lookat, player_moveto;

    private void Start()
    {
        player_movement = player.GetComponent<PlayerMovement>();
        inventory = GetComponent<Inventory>();
    }

    public void BeginShopping()
    {
        player.FreezeInput();
        player_movement.ForceIntoPosition(player_moveto, 4);
    }


}
