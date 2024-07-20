using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class EntryTile : MonoBehaviour
{
    public GenericData entryData;

    public NotebookHandler notebookHandler;

    public Image icon;
    public TMP_Text lable;
    public Button button;

    // Start is called before the first frame update
    private void Start()
    {
        if(entryData != null)
        {
            fillData();
        }
    }

    public void setData(GenericData data)
    {
        entryData = data;
        fillData();
        checkIfKnown();
    }

    private void fillData()
    {
        icon.sprite = entryData.icon;
        lable.text = entryData.Name;
    }

    private void checkIfKnown()
    {
        if(notebookHandler != null && notebookHandler.checkIfKnown(entryData))
        {
            button.interactable = true;
            icon.color = Color.white;
            lable.color = notebookHandler.textColor;
        }
        else
        {
            button.interactable = false;
            icon.color = Color.grey;
            lable.color = notebookHandler.textColorDisabled;
        }
    }

    public void goToEntryPage()
    {
        notebookHandler.goToEntryPage(entryData);
    }
}
