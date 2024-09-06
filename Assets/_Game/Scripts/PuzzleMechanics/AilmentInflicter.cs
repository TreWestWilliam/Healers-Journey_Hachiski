using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AilmentInflicter : MonoBehaviour
{
    public static AilmentInflicter Instance;

    [SerializeField] private List<NPC> npcs;
    [SerializeField] private List<AilmentIndex> ailments;
    [SerializeField] private Reputation reputation;

    [SerializeField] private float minDelayBetweenAilments;
    [SerializeField] private float maxDelayBetweenAilments;
    [SerializeField] private int TotalCured = 0;
    private int tierCount = 0;

    private List<AilmentData>[] inflictedAilments;
    private AilmentData[] lastInflictedAilment;
    private int[] ailmentsPerTier;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        lastInflictedAilment = new AilmentData[0];
        inflictedAilments = new List<AilmentData>[0];
        ailmentsPerTier = new int[0];

        updateTierCount();
    }

    private void Start()
    {
        foreach(NPC npc in npcs)
        {
            ailNPCAfterDelay(npc);
        }
    }

    private void updateTierCount()
    {
        int count = 0;
        foreach(AilmentIndex index in ailments)
        {
            if(index.tiers.Length > count)
            {
                count = index.tiers.Length;
            }
        }
        tierCount =  count;
        updateInflictedLengths();
        updateAilmentsPerTierCounts();
    }

    private void updateInflictedLengths()
    {
       if(tierCount > lastInflictedAilment.Length)
        {
            AilmentData[] temp = new AilmentData[tierCount];
            for(int i = 0; i < lastInflictedAilment.Length; i++)
            {
                temp[i] = lastInflictedAilment[i];
            }
            lastInflictedAilment = temp;
        }
       if(tierCount > inflictedAilments.Length)
        {
            List<AilmentData>[] temp = new List<AilmentData>[tierCount];
            for(int i = 0; i < tierCount; i++)
            {
                if(i < inflictedAilments.Length)
                {
                    temp[i] = inflictedAilments[i];
                }
                else
                {
                    temp[i] = new List<AilmentData>();
                }
            }
            inflictedAilments = temp;
        }
    }

    private void updateAilmentsPerTierCounts()
    {
        if(ailmentsPerTier.Length < tierCount)
        {
            ailmentsPerTier = new int[tierCount];
        }

        List<AilmentData>[] ailmentLists = new List<AilmentData>[tierCount];
        for(int i = 0; i < tierCount; i++)
        {
            ailmentLists[i] = new List<AilmentData>();
        }

        foreach(AilmentIndex index in ailments)
        {
            GenericData[] datas = index.getContents();
            foreach(GenericData data in datas)
            {
                AilmentData ailment = data as AilmentData;
                if(ailment != null  && !ailmentLists[ailment.tier].Contains(ailment))
                {
                    ailmentLists[ailment.tier].Add(ailment);
                }
            }
        }
        for(int i = 0; i < tierCount; i++)
        {
            ailmentsPerTier[i] = ailmentLists[i].Count;
        }
    }

    public void setAilmentIndex(AilmentIndex ailmentIndex)
    {
        ailments = new List<AilmentIndex>();
        ailments.Add(ailmentIndex);
        tierCount = ailmentIndex.tiers.Length;
        updateInflictedLengths();
        updateAilmentsPerTierCounts();
    }

    public void setAilmentIndex(AilmentIndex[] ailmentIndex)
    {
        ailments = new List<AilmentIndex>(ailmentIndex);
        updateTierCount();
    }

    public void setAilmentIndex(List<AilmentIndex> ailmentIndex)
    {
        ailments = ailmentIndex;
        updateTierCount();
    }

    public void addAilmentIndex(AilmentIndex ailmentIndex)
    {
        if(!ailments.Contains(ailmentIndex))
        {
            ailments.Add(ailmentIndex);
            if(ailmentIndex.tiers.Length > tierCount)
            {
                tierCount = ailmentIndex.tiers.Length;
                updateInflictedLengths();
            }
            updateAilmentsPerTierCounts();
        }
    }

    public void addAilmentIndex(AilmentIndex[] ailmentIndices)
    {
        bool addedIndex = false;
        bool increaseTierCount = false;
        foreach(AilmentIndex ailmentIndex in ailmentIndices)
        {
            if(!ailments.Contains(ailmentIndex))
            {
                addedIndex = true;
                ailments.Add(ailmentIndex);
                if(ailmentIndex.tiers.Length > tierCount)
                {
                    tierCount = ailmentIndex.tiers.Length;
                    increaseTierCount = true;
                }
            }
        }
        if(addedIndex)
        {
            if(increaseTierCount)
            {
                updateInflictedLengths();
            }
            updateAilmentsPerTierCounts();
        }
    }

    public void removeAilmentIndex(AilmentIndex ailmentIndex)
    {
        if(ailments.Remove(ailmentIndex))
        {
            updateTierCount();
        }
    }

    public void removeAilmentIndex(AilmentIndex[] ailmentIndices)
    {
        bool removed = false;
        foreach(AilmentIndex ailmentIndex in ailmentIndices)
        {
            removed |= ailments.Remove(ailmentIndex);
        }
        if(removed)
        {
            updateTierCount();
        }
    }

    public void addNPC(NPC npc)
    {
        npcs.Add(npc);
        ailNPCAfterDelay(npc);
    }

    public void removeNPC(NPC npc)
    {
        npcs.Remove(npc);
    }

    public void curedNPC(NPC npc, AilmentData ailment)
    {
        TotalCured++;
        inflictedAilments[ailment.tier].Remove(ailment);
        ailNPCAfterDelay(npc);

    }

    private AilmentData pickAilment()
    {
        List<AilmentData> options = new List<AilmentData>();

        for(int i = 0; i < tierCount; i++)
        {
            if(i > 0 && inflictedAilments[i - 1].Count == 0)
            {
                break;
            }
            if(i <= reputation.RepTier || inflictedAilments[i].Count == 0)
            {
                foreach(AilmentIndex index in ailments)
                {
                    if(index.tiers.Length > i)
                    {
                        foreach(GenericData data in index.tiers[i].data)
                        {
                            AilmentData ailment = data as AilmentData;
                            if(ailment != null && (ailment != lastInflictedAilment[i] || ailmentsPerTier[i] == 1) && !options.Contains(ailment))
                            {
                                options.Add(ailment);
                            }
                        }
                    }
                }
            }
        }

        if(options.Count == 0)
        {
            return null;
        }

        int ran = Random.Range(0, options.Count);

        return options[ran];
    }

    private void ailNPC(NPC npc)
    {
        if(npc.ailment == null && npcs.Contains(npc))
        {
            AilmentData ailment = pickAilment();
            inflictedAilments[ailment.tier].Add(ailment);
            lastInflictedAilment[ailment.tier] = ailment;

            npc.developeAilment(ailment);
        }    
    }

    private void ailNPCAfterDelay(NPC npc)
    {
        float delay = Random.Range(minDelayBetweenAilments, maxDelayBetweenAilments);

        StartCoroutine(delayedAilment(npc, delay));
    }

    public int GetTotalCured() { return TotalCured; }

    public IEnumerator delayedAilment(NPC npc, float delay)
    {
        yield return new WaitForSeconds(delay);

        ailNPC(npc);
    }
}
