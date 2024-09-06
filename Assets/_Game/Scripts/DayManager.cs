using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DayManager : MonoBehaviour
{
    public List<DayStuff> DayChecks;
    private AilmentInflicter _AilmentInflictor;
    private int CurrentDay;
    
    // Start is called before the first frame update
    void Start()
    {
        CurrentDay = 0;
        if (_AilmentInflictor == null) 
        {
            _AilmentInflictor = FindObjectOfType<AilmentInflicter>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoNextDay() 
    {

        foreach (DayStuff DS in DayChecks) 
        {
            if (!DS.HasBeenDone) 
            {
                if (DS.npcCheck != null) 
                {
                    if (DS.npcCheck.ailment == null && _AilmentInflictor.GetTotalCured() >= DS.requiredTotal) // If we're good to go
                    {
                        foreach (GameObject GO in DS.EnableObjects)
                        {
                            GO.SetActive(true);
                        }
                        foreach (GameObject GO in DS.DisableObjects)
                        {
                            GO.SetActive(false);
                        }
                    }
                    DS.OnConditionsMet.Invoke();
                }
                else 
                {
                    if (_AilmentInflictor.GetTotalCured() >= DS.requiredTotal)
                    {
                        foreach (GameObject GO in DS.EnableObjects)
                        {
                            GO.SetActive(true);
                        }
                        foreach (GameObject GO in DS.DisableObjects)
                        {
                            GO.SetActive(false);
                        }
                        DS.OnConditionsMet.Invoke();
                    }
                }
                
            }
        }
        CurrentDay++;
        
    }


    public int GetDay() { return CurrentDay; }
}
[System.Serializable]
public struct DayStuff 
{
    public NPC npcCheck;
    public int requiredTotal;
    public bool HasBeenDone;
    public GameObject[] EnableObjects;
    public GameObject[] DisableObjects;
    public UnityEvent OnConditionsMet;
}