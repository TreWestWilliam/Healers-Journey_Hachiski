using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
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
    [SerializeField] private StringRef LocationString;

    [SerializeField] private SVGImage icon;

    public GameObject invManager;

    private void Awake()
    {
        if(icon != null)
        {
            icon.sprite = item.icon;
        }
        if(LocationString != null && LocationString.Value != null && LocationString.Value != "")
        {
            for(int i = 0; i < item.locationsFound.Length; i++)
            {
                if(LocationString.Value == item.locationsFound[i].Value)
                {
                    locationIndex = i;
                }
            }
        }
        //Try to make sure we have invmanager set
        invManager = invManager != null ? invManager : FindObjectOfType<InventoryManager>().gameObject;

    }

    public void Engage(PlayerMovement player)
    {
        InventoryManager inv = invManager.GetComponent<InventoryManager>();
        inv.AddItem(item, numIngredients);
        if(item is IngredientData)
        {
            player.PickupObjectLow();
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
