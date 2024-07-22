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
        listedName.text = itemContained.GetItemName();
        listedQuantity.text = itemQuantity.ToString();
    }
}
