using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int numIngredients;
    [SerializeField] private string itemName;
    [SerializeField] private GameObject self;
    public GameObject invManager;

    public void Collect()
    {
        InventoryManager inv = invManager.GetComponent<InventoryManager>();
        inv.AddItem(itemName, numIngredients);
        self.SetActive(false);
    }


}
