using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Item itemContained;
    [SerializeField] private int itemQuantity;
    [SerializeField] private TMP_Text listedName;
    [SerializeField] private TMP_Text listedQuantity;

    private void Start()
    {
        UpdateDisplay();
    }

    public void SetItem(Item item)
    {
        itemContained = item;
    }

    public void SetQuantity(int quantity)
    {
        itemQuantity = quantity;
    }

    public void UpdateDisplay()
    {
        if(itemContained == null)
        {
            listedName.text = string.Empty;
            listedQuantity.text = string.Empty;
        }
        else
        {
            listedName.text = itemContained.GetItemName();
            listedQuantity.text = itemQuantity.ToString();
        }
    }
}
