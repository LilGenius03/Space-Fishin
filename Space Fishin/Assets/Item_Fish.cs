using UnityEngine;

[CreateAssetMenu(fileName = "New Fish", menuName = "Items/Fish")]
public class Item_Fish : Item
{
    public FishType fish_type;
    public GameObject fish_model;
    public int bait_giver;
}
