using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Items/New Item")]
public class Item : ScriptableObject
{
    [Header("Identity")]
    public string itm_name;
    public Sprite itm_icon;

    [Header("Stats")]
    public int price;
}
