using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastTravelNPC : MonoBehaviour, IInteractable
{
    public Transform TeleportPoint;
    public TransitionManager TM;
    public CharacterController ply;
    public bool IsTraveling = false;

    public void Disengage(PlayerMovement player)
    {
        
    }

    public void Engage(PlayerMovement player)
    {
        if (IsTraveling) { return; }
        IsTraveling = true;

        CharacterController CC = player.GetComponent<CharacterController>();
        ply = CC;
        TM.StartTravel(this);

        
    }

    public void Warp() 
    {
        ply.enabled = false;
        ply.transform.position = TeleportPoint.position;
        ply.enabled = true;
    }

    public void EndTravel() { IsTraveling = false; }

    // Start is called before the first frame update
    void Start()
    {
        if (TM == null) { TM = FindObjectOfType<TransitionManager>(); }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
