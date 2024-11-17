using TMPro;
using UnityEngine;

public class UISHOP : MonoBehaviour
{
    public ItemList itemList;
    public GameObject[] fish_buttons;
    public TextMeshProUGUI[] fish_counts;
    bool init;

    private void Start()
    {
        UpdateShop();
    }

    private void OnEnable()
    {
        if (init)
            UpdateShop();
    }

    public void UpdateShop()
    {
        for(int i = 0; i < itemList.fish_array.Length; i++)
        {
            Debug.Log("Shop Updated: " + itemList.fish_array[i]);
            if (Inventory.instance.inventory.ContainsKey(itemList.fish_array[i]))
            {
                fish_buttons[i].SetActive(true);
                fish_counts[i].text = Inventory.instance.inventory[itemList.fish_array[i]].ToString();
            }
            else
                fish_buttons[i].SetActive(false);
        }
        init = true;
    }
}
