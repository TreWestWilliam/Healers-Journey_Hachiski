using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TierIndex
{
    public GenericData[] data;
}

public abstract class IndexData : Data
{
    public string title;
    public TierIndex[] tiers;
}
