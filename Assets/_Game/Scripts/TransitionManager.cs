using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionManager : MonoBehaviour
{
    public Animator FastTravelAnimator;
    public void StartTravel(FastTravelNPC npc)
    {
        FastTravelAnimator.SetBool("Active", true);
        npc.Invoke("Warp", 1.2f);
        Invoke("EndTravel", 1.3f);
        npc.Invoke("EndTravel", 2.2f);
    }
    public void EndTravel() 
    {
        FastTravelAnimator.SetBool("Active", false);
    }

    public void StartSleep() 
    {
        FastTravelAnimator.SetBool("Active", true);
        Invoke("EndSleep", 1f);
    }
    public void EndSleep() 
    {
        FastTravelAnimator.SetBool("Active", false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
