using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class CureTile : MonoBehaviour
{
    public Cure cure;

    public NotebookHandler notebookHandler;

    public Image icon;
    public TMP_Text lable;

    // Start is called before the first frame update
    private void Start()
    {
        if(cure != null)
        {
            fillData();
        }
    }

    public void setData(Cure data)
    {
        cure = data;
        fillData();
    }

    private void fillData()
    {
        icon.sprite = cure.cureSprite;
        lable.text = cure.cureName;
    }
}
