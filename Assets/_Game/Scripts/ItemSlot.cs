using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Item itemContained;
    [SerializeField] private int itemQuantity;
    [SerializeField] private TMP_Text listedName;
    [SerializeField] private TMP_Text listedQuantity;
    [SerializeField] private Image itemIcon;

    private void Awake() {
        itemIcon = GetComponent<Image>();
    }

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
            itemIcon.sprite = null;
        }
        else
        {
            listedName.text = itemContained.GetItemName();
            listedQuantity.text = itemQuantity.ToString();
            itemIcon.sprite = itemContained.GetIcon();
        }
    }
}
