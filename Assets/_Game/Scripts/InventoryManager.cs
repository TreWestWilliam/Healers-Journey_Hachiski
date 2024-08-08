using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

[Serializable]
public class ItemQuantity : IComparable<ItemQuantity>
{
    public ItemData item;
    public int quantity;

    public ItemQuantity(ItemData item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }

    public int CompareTo(ItemQuantity obj)
    {
        if (obj == null) return 1;
        if(item == null)
        {
            if(obj.item == null)
            {
                return 0;
            }
            return -1;
        }
        return item.CompareTo(obj.item);
    }
}

public class InventoryManager : MonoBehaviour
{
    public List<ItemQuantity> items; 
    public List<Item> itemList;
    public List<int> itemQuantities;
    public Records records;

    [SerializeField] private IngredientIndex ingredientIndex;
    //private List<IngredientData> itemIndex;

    // Start is called before the first frame update
    void Start()
    {
        items = new List<ItemQuantity>();
        itemList = new List<Item>();
        itemQuantities = new List<int>();
        /*itemIndex = new List<IngredientData>();

        
        // Adds items for every ingredient from index to itemIndex.
        GenericData[] indexData = dataIndex.getContents();
        foreach(GenericData data in indexData)
        {
            IngredientData ingredient = data as IngredientData;
            if(ingredient != null)
            {
                itemIndex.Add(ingredient);
            }
        }*/
    }

    public void AddItem(ItemData item, int num)
    {
        
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].item == item)
            {
                items[i].quantity += num;
                return;
            }
        }
        /*int i = 0;
        foreach (Item currentItem in itemList)
        {
            if (currentItem.checkItem(item))
            {
                itemQuantities[i] += num;
                return;
            }
            i++;
        }

        foreach (Item newItem in itemIndex)
        {
            if(newItem.checkIngredient(ingredient))
            {
                itemList.Add(newItem);
                itemQuantities.Add(num);
                return;
            }
        }*/
        if(ingredientIndex.contains(item))
        {
            ItemQuantity newItem = new ItemQuantity(item, num);
            items.Add(newItem);
            items.Sort();
            return;
        }
        Debug.Log("Item not found");
    }

    // Returns true if item successfully removed, false if not enough of item or if not in inventory
    public bool RemoveItem(ItemData item, int num)
    {
        /*int i = 0;
        foreach (Item currentItem in itemList)
        {
            if(currentItem.checkItem(ingredient))
            {
                if (itemQuantities[i] < num) return false;

                itemQuantities[i] -= num;
                if (itemQuantities[i] <= 0)
                {
                    itemList.RemoveAt(i);
                    itemQuantities.RemoveAt(i);
                }
                return true;
            }
            i++;
        }
        return false;*/

        int i = 0;
        foreach(ItemQuantity currentItem in items)
        {
            if(currentItem.item == item)
            {
                if(currentItem.quantity < num) return false;

                items[i].quantity -= num;
                if(items[i].quantity <= 0)
                {
                    items.RemoveAt(i);
                }
                return true;
            }
            i++;
        }
        return false;
    }

    public int getQuantity(ItemData item)
    {
        foreach(ItemQuantity currentItem in items)
        {
            if(currentItem.item == item)
            {
                return currentItem.quantity;
            }
        }
        return 0;
    }

    public void UpdateInventory()
    {
        GameObject[] itemSlots = GameObject.FindGameObjectsWithTag("Item Slot");
        int i = 0;
        foreach (GameObject itemSlot in itemSlots)
        {
            /*if(itemList.Count > i)
            {
                itemSlot.transform.GetComponent<ItemSlot>().SetItem(itemList[i]);
                itemSlot.transform.GetComponent<ItemSlot>().SetQuantity(itemQuantities[i]);
                itemSlot.transform.GetComponent<ItemSlot>().UpdateDisplay();
            }*/
            if(items.Count > i)
            {
                itemSlot.transform.GetComponent<ItemSlot>().SetItem(items[i].item);
                itemSlot.transform.GetComponent<ItemSlot>().SetQuantity(items[i].quantity);
                itemSlot.transform.GetComponent<ItemSlot>().UpdateDisplay();
            }
            else
            {
                itemSlot.transform.GetComponent<ItemSlot>().SetItem((Item)null);
                itemSlot.transform.GetComponent<ItemSlot>().SetQuantity(0);
                itemSlot.transform.GetComponent<ItemSlot>().UpdateDisplay();
            }
            i++;
        }
    }
}
