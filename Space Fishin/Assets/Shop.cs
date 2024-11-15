using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] PlayerController player;
    PlayerMovement player_movement;
    Inventory player_inventory;

    [SerializeField] int player_cash;

    [SerializeField] GameObject shop_ui;
    [SerializeField] Transform player_lookat, player_moveto;

    private void Start()
    {
        player_movement = player.GetComponent<PlayerMovement>();
        player_inventory = player.GetComponent<Inventory>();
    }

    public void BeginShopping()
    {
        player.FreezeInput();
        player_movement.ForceIntoPosition(player_moveto, 4);
        shop_ui.SetActive(true);
        Cursor.visible = true;
    }

    public void BuySomething(Item itm_to_buy)
    {
        if (player_cash - itm_to_buy.buy_price >= 0 && player_inventory.AddItem(itm_to_buy))
        {
            player_cash -= itm_to_buy.buy_price;
        }
        else
        {

        }
    }

    public void SellSomething(Item itm_to_sell)
    {
        if (player_inventory.RemoveItem(itm_to_sell))
        {
            player_cash += itm_to_sell.sell_price;
        }
    }
}
