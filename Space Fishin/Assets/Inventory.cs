using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// THIS IS AN INCOMPLETE INVENTORY SCRIPT - It is more advanced than one used
/// </summary>
public class Inventory : MonoBehaviour
{

    public static Inventory instance;
    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }

    public ItemList full_item_list;

    public Dictionary<Item_Fish, int> inventory = new Dictionary<Item_Fish, int>();

    public int[] baits = new int[6]; // cod, jar, othercommon, eel, other rare, shell


    [SerializeField] int inventory_size = 10;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
            AddItem(full_item_list.fish_array[1]);
    }

    public void AddBait(int bait_type, int amount)
    {
        baits[bait_type] += amount;
    }

    public void RemovedBait(int bait_type, int amount)
    {
        baits[bait_type] -= amount;
    }

    public bool AddItem(Item_Fish new_item)
    {
        if (inventory.Count + 1 > inventory_size)
            return false;

        if (inventory.ContainsKey(new_item))
        {
            if (inventory[new_item] + 1 > new_item.max_amount && new_item.max_amount != -1)
            {
                Debug.Log(PrintInv());
                return false;
            }
            else
            {
                inventory[new_item]++;
            }
                
        }
        else
            inventory.Add(new_item, 1);
        Debug.Log(PrintInv());
        return true;
    }

    public bool RemoveItem(Item_Fish itm)
    {
        if (!inventory.ContainsKey(itm))
            return false;

        inventory[itm]--;

        if (inventory[itm] <= 0)
            inventory.Remove(itm);

        Debug.Log(PrintInv());
        return true;
    }

    string PrintInv()
    {
        string s = "Inventory: \n";
        foreach(KeyValuePair<Item_Fish, int> itm in inventory)
        {
            s += itm.Key.itm_name + ": " + itm.Value + "\n";
        }
        return s;
    }
}