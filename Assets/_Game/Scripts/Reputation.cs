using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reputation : MonoBehaviour
{
    public int CurrentRep { get; private set; } = 0;

    public void setRep(int rep)
    {
        CurrentRep = rep;
    }

    public void adjustRep(int rep)
    {
        setRep(CurrentRep + rep);
    }
}
