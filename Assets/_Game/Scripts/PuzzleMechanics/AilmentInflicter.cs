using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AilmentInflicter : MonoBehaviour
{
    public static AilmentInflicter Instance;

    [SerializeField] private List<NPC> npcs;
    [SerializeField] private AilmentIndex ailments;
    [SerializeField] private Reputation reputation;

    [SerializeField] private float minDelayBetweenAilments;
    [SerializeField] private float maxDelayBetweenAilments;
    [SerializeField] private int TotalCured = 0;

    private List<AilmentData>[] inflictedAilments;
    private AilmentData[] lastInflictedAilment;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        lastInflictedAilment = new AilmentData[ailments.tiers.Length];
        inflictedAilments = new List<AilmentData>[ailments.tiers.Length];
        for(int i = 0; i < inflictedAilments.Length; i++)
        {
            inflictedAilments[i] = new List<AilmentData>();
        }
    }

    private void Start()
    {
        foreach(NPC npc in npcs)
        {
            ailNPCAfterDelay(npc);
        }
    }

    public void setAilmentIndex(AilmentIndex ailmentIndex)
    {
        ailments = ailmentIndex;
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

        for(int i = 0; i < ailments.tiers.Length; i++)
        {
            if(i > 0 && inflictedAilments[i - 1].Count == 0)
            {
                break;
            }
            if(i <= reputation.RepTier || inflictedAilments[i].Count == 0)
            {
                foreach(GenericData data in ailments.tiers[i].data)
                {
                    AilmentData ailment = data as AilmentData;
                    if(ailment != null && (ailment != lastInflictedAilment[i] || ailments.tiers[i].data.Length == 1))
                    {
                        options.Add(ailment);
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
