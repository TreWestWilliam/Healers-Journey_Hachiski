using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine.UI;

public class IndexPage : MonoBehaviour
{
    public IndexData index;

    public NotebookHandler notebookHandler;

    public IndexTierTile indexTierTilePrefab;

    public TMP_Text indexTitle;
    public Transform entriesSection;


    public FontRef titleFont;
    public ColorRef titleColor;

    public FontRef tiersFont;
    public ColorRef tiersColor;

    // Start is called before the first frame update
    private void Start()
    {
        indexTitle.color = titleColor;
        indexTitle.font = titleFont;
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void openIndex(IndexData chosenIndex)
    {
        notebookHandler.clearSection(entriesSection.gameObject);
        index = chosenIndex;
        indexTitle.text = index.title;

        for(int i = 0; i < chosenIndex.tiers.Length; i++)
        {
            TierIndex tier = chosenIndex.tiers[i];
            IndexTierTile tierEntries = Instantiate(indexTierTilePrefab, entriesSection);
            tierEntries.name = notebookHandler.repHandler.getTierName(i) + " Index";
            tierEntries.tierLable.text = notebookHandler.repHandler.getTierName(i);
            tierEntries.tierLable.font = tiersFont;
            tierEntries.tierLable.color = tiersColor;
            EntryTile lastTile = null;
            foreach(GenericData data in tier.data)
            {
                lastTile = notebookHandler.createEntryTile(data, tierEntries.entryGrid.transform);
            }
            if(lastTile != null)
            {
                StartCoroutine(notebookHandler.updateGrid(tierEntries.entryGrid, tier.data.Length));
            }

        }
    }
}