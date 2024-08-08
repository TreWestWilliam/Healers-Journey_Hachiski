using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Collectable : MonoBehaviour, IInteractable
{
    [SerializeField] private int numIngredients;
    [SerializeField] private ItemData item;
    [SerializeField] private int locationIndex = -1;
    [SerializeField] private GameObject self;
    [SerializeField] private InteractNotif notif;
    [SerializeField] private float respawnDelay = 5f;


    public GameObject invManager;

    public void Engage(PlayerMovement player)
    {
        InventoryManager inv = invManager.GetComponent<InventoryManager>();
        inv.AddItem(item, numIngredients);
        if(item is IngredientData)
        {
            if(locationIndex >= 0 && item.locationsFound != null && locationIndex < item.locationsFound.Length)
            {
                inv.records.discoverIngredientSymptoms(item as IngredientData, locationIndex);
            }
            else
            {
                inv.records.discoverIngredientSymptoms(item as IngredientData);
            }
        }
        Invoke("respawn", respawnDelay);
        notif.SetCollectNotifVisible(false);
        self.SetActive(false);
    }

    public void Disengage(PlayerMovement player)
    {
        return;
    }

    private void respawn()
    {
        self.SetActive(true);
        if(notif != null)
        {

        }
    }
}
