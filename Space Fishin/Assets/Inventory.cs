using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Item> inventory = new List<Item>();
    [SerializeField] int inventory_size = 10;


    public bool AddItem(Item new_item)
    {
        if (inventory.Count + 1 > inventory_size)
            return false;

        inventory.Add(new_item);
        return true;
    }

    public bool RemoveItem(Item itm)
    {
        if (!inventory.Contains(itm))
            return false;

        inventory.Remove(itm);
        return true;
    }
}
