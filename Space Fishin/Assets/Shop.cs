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

    [SerializeField] Animator mac_anim;

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
        gameplay_ui.SetActive(false);
        player_movement.ForceIntoPosition(player_moveto);
        player_movement.fishrod_graphics.SetActive(false);
        player_movement.line_graphics.SetActive(false);
        player.bob.SetActive(false);
        mac_anim.SetBool("entered", true);
        float time_delay = 0f;
        yield return new WaitForSecondsRealtime(0.01f);
        Debug.Log(mac_anim.GetCurrentAnimatorStateInfo(0).shortNameHash);
        while (mac_anim.GetCurrentAnimatorStateInfo(0).shortNameHash == 1537617548)
        {
            Debug.Log("hey?");
            player_movement.ForceIntoPosition(player_moveto);
            time_delay += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("yo");
        yield return new WaitForSecondsRealtime(3f);
        Debug.Log("bro");

        while (cam.fieldOfView > 18)
        {
            cam.fieldOfView -= zoom_speed * Time.deltaTime;
            player_movement.ForceIntoPosition(player_moveto);
            yield return null;
        }
        shop_ui.SetActive(true);
        gameplay_ui.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LeaveShopping()
    {
        StartCoroutine(ShoppingEnding());
    }

    IEnumerator ShoppingEnding()
    {
        shop_ui.SetActive(false);
        mac_anim.SetBool("entered", false);
        while (cam.fieldOfView <= 90)
        {
            cam.fieldOfView += 50 * Time.deltaTime;
            player_movement.ForceIntoPosition(player_moveto);
            yield return null;
        }
        cam.fieldOfView = 90;
        player.UnFreezeInput();
        gameplay_ui.SetActive(true);
        player_movement.fishrod_graphics.SetActive(true);
        player_movement.line_graphics.SetActive(true);
        player.bob.SetActive(true);
        player_movement.FreePlayerFromPosition();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
