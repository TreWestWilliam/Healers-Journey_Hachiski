using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //CLARIFY: What other properties does an Item need?
    protected string itemName;
    protected Image inventoryIcon;

    public string GetItemName()
    {
        return itemName;
    }
}
