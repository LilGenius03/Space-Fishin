using UnityEngine;

public abstract class Item : ScriptableObject
{
    [Header("Identity")]
    public string itm_name;
    public Sprite itm_icon;

    [Header("Stats")]
    public int max_amount;
    public int buy_price;
    public int sell_price;
}
