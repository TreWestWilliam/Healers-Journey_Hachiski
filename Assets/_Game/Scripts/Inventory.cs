using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> inventorySlots;

    private void Awake() {
        inventorySlots = new List<ItemSlot>();
    }
}
