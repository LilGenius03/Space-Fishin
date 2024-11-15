using UnityEngine;

[CreateAssetMenu(fileName = "New Item List", menuName = "Items/List")]
public class ItemList : ScriptableObject
{
    [Header("Fish")]
    public Item jar_fish;
    public Item shell_fish;
    public Item eel_fish;
    public Item cod_fish;
    public Item tbd_fish1;
    public Item tbd_fish2;
    public Item debug_fish;

    [Header("Other")]
    public Item fuel;
}
