using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private int numIngredients;
    [SerializeField] private string itemName;
    [SerializeField] private GameObject self;
    public GameObject invManager;

    public void Engage()
    {
        InventoryManager inv = invManager.GetComponent<InventoryManager>();
        inv.AddItem(itemName, numIngredients);
        self.SetActive(false);
    }

    public void Disengage()
    {
        return;
    }
}
