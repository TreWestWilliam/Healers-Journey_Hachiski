using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using TMPro;
//using UnityEditor.Experimental.GraphView;

public class PuzzleInventorySlot : MonoBehaviour
{
    [SerializeField] private SVGImage icon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text quantityText;
    [SerializeField] private ItemDragger dragger;

    public void setUpSlot(ItemData item, int quantity)
    {
        name = "Item Slot : " + item.Name;
        icon.sprite = item.icon;
        itemName.text = item.Name;
        quantityText.text = quantity.ToString();
        dragger.setItem(item);
    }

    public void updateQuantity(int  quantity)
    {
        quantityText.text = quantity.ToString();
        if(quantity > 0)
        {
            dragger.setDisableAfterDrag(false);
        }
        else
        {
            dragger.setDisableAfterDrag(true);
        }
    }
}
