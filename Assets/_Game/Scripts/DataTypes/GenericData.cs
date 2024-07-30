using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class GenericData : Data, IComparable<GenericData>
{
    [TextArea(3,5)]
    public string Description;

    [TextArea(3, 5)]
    public string UnlockedDescription;

    public int tier;

    public SpriteRef icon;

    public abstract DiscoveryTypeIndices getDataIndex(GenericData data);
    public abstract DiscoveryTypeIndices[] getDataIndex(GenericData[] data);

    public int CompareTo(GenericData other)
    {
        if (other == null) return 1;

        if(tier != other.tier)
        {
            return tier.CompareTo(other.tier);
        }

        return Name.CompareTo(other.Name);
    }

#if UNITY_EDITOR
    public abstract void sortArrays();

    public abstract void reciprocateData();
#endif
}
