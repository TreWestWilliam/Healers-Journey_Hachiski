using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //CLARIFY: What other properties does an Item need?
    protected string itemName;
    [SerializeField] protected Texture2D inventoryIcon;

    public string GetItemName()
    {
        return itemName;
    }

    public Sprite GetIcon() {
        if (inventoryIcon == null) return null;
        return Sprite.Create(inventoryIcon, new Rect(0f, 0f, inventoryIcon.width, inventoryIcon.height), new Vector2(0f, 0f), 100f);
    }
}
