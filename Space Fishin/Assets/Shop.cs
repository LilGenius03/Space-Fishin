using System.Collections;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] PlayerController player;
    PlayerMovement player_movement;
    Inventory player_inventory;
    [SerializeField] Camera cam;

    [SerializeField] int player_cash;

    [SerializeField] GameObject shop_ui, gameplay_ui;
    [SerializeField] Transform player_lookat, player_moveto;

    [SerializeField] UISHOP userinterfaceshop;

    [SerializeField] float zoom_speed;

    public ItemList full_item_list;

    private void Start()
    {
        player_movement = player.GetComponent<PlayerMovement>();
        player_inventory = player.GetComponent<Inventory>();
    }

    public void BeginShopping()
    {
        StartCoroutine(ShoppingBegin());
    }

    IEnumerator ShoppingBegin()
    {
        player.FreezeInput();
        player_movement.ForceIntoPosition(player_moveto);
        while(cam.fieldOfView > 18)
        {
            cam.fieldOfView -= zoom_speed * Time.deltaTime;
            player_movement.ForceIntoPosition(player_moveto);
            yield return null;
        }
        shop_ui.SetActive(true);
        gameplay_ui.SetActive(false);
        Cursor.visible = true;
    }

    public void BuyBait(Item_Fish fish)
    {
        if(player_inventory.RemoveItem(fish))
            player_inventory.AddBait(fish.bait_giver, 1);
        userinterfaceshop.UpdateShop();
    }

    public void BuySomething(Item_Fish itm_to_buy)
    {
        if (player_cash - itm_to_buy.buy_price >= 0 && player_inventory.AddItem(itm_to_buy))
        {
            player_cash -= itm_to_buy.buy_price;
        }
        else
        {

        }
    }

    public void SellSomething(Item_Fish itm_to_sell)
    {
        if (player_inventory.RemoveItem(itm_to_sell))
        {
            player_cash += itm_to_sell.sell_price;
        }
    }
}
