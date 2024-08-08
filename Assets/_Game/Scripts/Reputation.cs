using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reputation : MonoBehaviour
{
    public int CurrentRep { get; private set; } = 0;

    public int RepTier { get; private set; } = 0;

    public int highestTierReached { get; private set; } = 0;

    public int CurrentTierRep => GetCurrentTierRep();

    public float ratioToNextTier => GetRatioToNextTier();

    public bool atMaxTier => checkAtMaxTier();

    public int[] ReputationPerTier = { 1000, 2000, 3000, 4000 };

    public ReputationUIHandler ui;

    public void setRep(int rep)
    {
        CurrentRep = rep;
        RepTier = getTierOfRep(rep);
        if(RepTier > highestTierReached)
        {
            highestTierReached = RepTier;
        }
        if(ui != null)
        {
            ui.setReputation();
        }
    }

    public void setHighestTierReached(int i)
    {
        if(i >= ReputationPerTier.Length)
        {
            i = ReputationPerTier.Length - 1;
        }
        highestTierReached = i;
    }

    public void adjustRep(int rep)
    {
        setRep(CurrentRep + rep);
    }

    private bool checkAtMaxTier()
    {
        return RepTier == ReputationPerTier.Length;
    }

    private int GetCurrentTierRep()
    {
        return CurrentRep - getTotalRepOfTier(RepTier);
    }

    private float GetRatioToNextTier()
    {
        if(atMaxTier)
        {
            return 1.0f;
        }

        return (float)(CurrentRep - getTotalRepOfTier(RepTier)) / (float)ReputationPerTier[RepTier];
    }

    private int getTierOfRep(int rep)
    {
        for(int i = 0; i < ReputationPerTier.Length; i++)
        {
            if(ReputationPerTier[i] > rep)
            { 
                return i;
            }
            rep -= ReputationPerTier[i];
        }

        return ReputationPerTier.Length;
    }

    private int getTotalRepOfTier(int tier)
    {
        if(tier > ReputationPerTier.Length || tier < 0)
        {
            throw new System.ArgumentOutOfRangeException($"{tier} is out of reputation tier range.");
        }

        int rep = 0;
        for(int i = 0; i < tier; i++)
        {
            rep += ReputationPerTier[i];
        }
        return rep;
    }
}
