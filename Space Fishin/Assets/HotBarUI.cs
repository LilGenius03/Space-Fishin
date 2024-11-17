using System.Collections;
using TMPro;
using UnityEngine;

public class HotBarUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI[] fish_counters;
    [SerializeField] Inventory inv;
    Coroutine co;
    [SerializeField] ItemList fullitemlist;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //inv = Inventory.instance;
    }

    // Update is called once per frame
    public void UpdateUI()
    {
        for(int i = 0; i < fish_counters.Length; i++)
        {
            if (inv.inventory.ContainsKey(fullitemlist.fish_array[i]))
                fish_counters[i].text = inv.inventory[fullitemlist.fish_array[i]].ToString();
            else
                fish_counters[i].text = 0.ToString();
        }
    }

    public void ShowUI()
    {
        StopAllCoroutines();
        UpdateUI();
        co = StartCoroutine(TempShow());
    }

    public IEnumerator TempShow()
    {
        gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2f);
        gameObject.SetActive(false);

    }
}
