using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class TierIndex
{
    public GenericData[] data = { };
}

public abstract class IndexData : Data
{
    public string title;
    public TierIndex[] tiers;

    public GenericData[] getContents()
    {
        List<GenericData> fullList = new List<GenericData>();

        for(int i = 0; i < tiers.Length; i++)
        {
            if(tiers[i] == null)
            {
                tiers[i] = new TierIndex();
            }
            if(tiers[i].data == null)
            {
                tiers[i].data = new GenericData[0];
            }
            foreach(GenericData data in tiers[i].data)
            {
                if(data != null && !fullList.Contains(data))
                {
                    fullList.Add(data);
                }
            }
        }
        fullList.Sort();

        return fullList.ToArray();
    }

    public bool contains(GenericData data)
    {
        for(int i = 0; i < tiers.Length; i++)
        {
            if(tiers[i] != null && tiers[i].data != null)
            {
                foreach(GenericData checkData in tiers[i].data)
                {
                    if(data == checkData)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

#if UNITY_EDITOR
    public void sortIndex()
    {
        GenericData[] fullList = getContents();

        int tierCount = tiers.Length;
        if(tierCount < fullList.Last().tier + 1)
        {
            tierCount = fullList.Last().tier + 1;
        }

        tiers = new TierIndex[tierCount];

        for(int i = 0; i < tiers.Length; i++)
        {
            tiers[i] = new TierIndex();
        }

        List<GenericData> tierList = new List<GenericData>();
        foreach(GenericData data in fullList)
        {
            if(tierList.Count > 0 && tierList[0].tier != data.tier)
            {
                tiers[tierList[0].tier].data = tierList.ToArray();
                tierList.Clear();
            }
            tierList.Add(data);
        }
        if(tierList.Count > 0)
        {
            tiers[tierList[0].tier].data = tierList.ToArray();
        }
    }
#endif
}
