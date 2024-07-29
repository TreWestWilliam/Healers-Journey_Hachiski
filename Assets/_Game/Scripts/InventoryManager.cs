using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public List<Item> itemList;
    public List<int> itemQuantities;

    [SerializeField] private GameObject self;
    private List<Item> itemIndex;

    // Start is called before the first frame update
    void Start()
    {
        itemList = new List<Item>();
        itemQuantities = new List<int>();
        itemIndex = new List<Item>();

        // Adds all attached Items to index
        foreach (Component comp in self.GetComponentsInChildren<Component>())
        {
            if (comp.GetType().IsSubclassOf(typeof(Item)))
            {
                itemIndex.Add(comp as Item);
            }
        }
    }

    public void AddItem(string itemName, int num)
    {
        int i = 0;
        foreach (Item currentItem in itemList)
        {
            if (currentItem.GetItemName() == itemName)
            {
                itemQuantities[i] += num;
                return;
            }
            i++;
        }
        
        foreach (Item newItem in itemIndex)
        {
            if(newItem.GetItemName() == itemName)
            {
                itemList.Add(newItem);
                itemQuantities.Add(num);
                return;
            }
        }
        Debug.Log("Item not found");
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
