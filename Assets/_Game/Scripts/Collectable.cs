using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [SerializeField] private int numIngredients;
    [SerializeField] private Item item;
    [SerializeField] private GameObject self;
    public GameObject invManager;

    [SerializeField] private GameObject collectNotif;

    private void Update() {
        SetCollectNotifVisible(false);
    }

    public void Collect()
    {
        InventoryManager inv = invManager.GetComponent<InventoryManager>();
        inv.AddItem(item, numIngredients);
        self.SetActive(false);
    }

    public void SetCollectNotifVisible(bool visible) {
        collectNotif.SetActive(visible);
    }


}
