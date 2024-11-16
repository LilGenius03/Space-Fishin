using UnityEngine;

[CreateAssetMenu(fileName = "New Bait", menuName = "Items/Bait")]
public class Item_Bait : Item
{
    public int baitindex;
    public FishType[] attract_types_of_fish;
}
