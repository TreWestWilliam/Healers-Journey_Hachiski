using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> itemList;
    public List<int> itemQuantities;

    // Start is called before the first frame update
    void Start()
    {
        itemList = new List<Item>();
        itemQuantities = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item, int num)
    {
        int i = 0;
        foreach (Item currentItem in itemList)
        {
            if (currentItem.GetItemName() == item.GetItemName())
            {
                itemQuantities[i] += num;
                return;
            }
            i++;
        }
        itemList.Add(item);
        itemQuantities.Add(num);
    }

    public void UpdateInventory()
    {
        GameObject[] itemSlots = GameObject.FindGameObjectsWithTag("Item Slot");
        int i = 0;
        foreach (GameObject itemSlot in itemSlots)
        {
            if(itemList.Count > i)
            {
                itemSlot.transform.GetComponent<ItemSlot>().SetItem(itemList[i]);
                itemSlot.transform.GetComponent<ItemSlot>().SetQuantity(itemQuantities[i]);
                itemSlot.transform.GetComponent<ItemSlot>().UpdateDisplay();
            }
            else
            {
                itemSlot.transform.GetComponent<ItemSlot>().SetItem(null);
                itemSlot.transform.GetComponent<ItemSlot>().SetQuantity(0);
                itemSlot.transform.GetComponent<ItemSlot>().UpdateDisplay();
            }
            i++;
        }
    }
}
