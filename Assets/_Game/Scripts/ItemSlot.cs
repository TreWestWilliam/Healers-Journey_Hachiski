using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Item itemContained;
    [SerializeField] private ItemData itemContainedData;
    [SerializeField] private int itemQuantity;
    [SerializeField] private TMP_Text listedName;
    [SerializeField] private TMP_Text listedQuantity;
    [SerializeField] private SVGImage itemIcon;

    private void Start()
    {
        UpdateDisplay();
    }

    public void SetItem(Item item)
    {
        itemContained = item;
        itemContainedData = null;
        if(itemContained == null)
        {
            gameObject.name = "Item Slot - Empty";
        }
        else
        {
            gameObject.name = "Item Slot - " + item.GetItemName();
        }
    }

    public void SetItem(ItemData item)
    {
        itemContainedData = item;
        itemContained = null;
        if(itemContainedData == null)
        {
            gameObject.name = "Item Slot - Empty";
        }
        else
        {
            gameObject.name = "Item Slot - " + item.Name;
        }
    }

    public void SetQuantity(int quantity)
    {
        itemQuantity = quantity;
    }

    public void UpdateDisplay()
    {
        //Debug.Log("Updating Display of " + name);
        if(itemContained == null && itemContainedData == null)
        {
            listedName.text = string.Empty;
            listedQuantity.text = string.Empty;
            itemIcon.sprite = null;
        }
        else if(itemContained != null)
        {
            //Debug.Log("Updating via item");
            listedName.text = itemContained.GetItemName();
            listedQuantity.text = itemQuantity.ToString();
            itemIcon.sprite = itemContained.GetIcon();
        }
        else
        {
            //Debug.Log("Updating via data");
            listedName.text = itemContainedData.Name;
            listedQuantity.text = itemQuantity.ToString();

            //Debug.Log("Icon is null? " + (itemContainedData.icon is null));
            Sprite sprite = itemContainedData.icon;
            //Debug.Log("Sprite is null? " + (sprite is null));
            itemIcon.sprite = sprite;
        }
    }
}
