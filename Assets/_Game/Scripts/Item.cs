using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    //CLARIFY: What other properties does an Item need?
    [SerializeField] protected ItemData item;

    private void Awake()
    {
    }

    public void setIngredient(ItemData itemType)
    {
        item = itemType;
    }

    public string GetItemName()
    {
        return item.Name;
    }

    public bool checkItem(ItemData ingredientType)
    {
        return item == ingredientType;
    }

    public Sprite GetIcon()
    {
        if (item == null) return null;
        return item.icon;
    }
}
