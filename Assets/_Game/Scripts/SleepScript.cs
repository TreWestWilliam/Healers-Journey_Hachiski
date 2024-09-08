using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class SleepScript : MonoBehaviour, IInteractable
{
    [SerializeField] private DayManager _DayManager;
    private TransitionManager TM;

    public void Disengage(PlayerMovement player)
    {
        // TODO: Animation Stuff for the day.
    }

    public void Engage(PlayerMovement player)
    {
        //Debug.Log("Bed is working");
        try
        {
            _DayManager.GoNextDay();
            TM.StartSleep();
        }
        catch (Exception e)
        {
            Debug.Log($"Exception: {e.Message}",this);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (_DayManager == null) 
        {
            _DayManager = GameObject.FindAnyObjectByType<DayManager>();
        }
        if (TM == null) 
        {
            TM = FindAnyObjectByType<TransitionManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
