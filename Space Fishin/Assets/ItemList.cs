using UnityEngine;

[CreateAssetMenu(fileName = "New Item List", menuName = "Items/List")]
public class ItemList : ScriptableObject
{
    [Header("Fish")]
    public Item_Fish jar_fish;
    public Item_Fish shell_fish;
    public Item_Fish eel_fish;
    public Item_Fish cod_fish;
    public Item_Fish tbd_fish1;
    public Item_Fish tbd_fish2;
    public Item_Fish debug_fish;
    public Item_Fish[] fish_array;

    public Color[] rarity_colours;

    [Header("Bait")]
    public Item_Bait bait_jarfish;
    public Item_Bait bait_shellfish;
    public Item_Bait bait_eelfish;
    public Item_Bait bait_codfish;
    public Item_Bait bait_tbdfish1;
    public Item_Bait bait_tbdfish2;
    public Item_Bait bait_debugfish;
    public Item_Bait[] bait_array;

    [Header("Other")]
    public Item fuel;
}
