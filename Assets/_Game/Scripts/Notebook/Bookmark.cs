using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bookmark : MonoBehaviour
{
    public NotebookHandler notebookHandler;

    public Data page;

    public Button exteriorBookmark;
    public Button interiorBookmark;

    public TMP_Text exteriorLabel;
    public TMP_Text interiorLabel;

    private void setActive(bool active)
    {
        exteriorBookmark.gameObject.SetActive(active);
        interiorBookmark.gameObject.SetActive(!active);
    }

    private void turnToPage()
    {
        setActive(false);
    }

    private void turnFromPage()
    {
        setActive(true);
    }

    private void setInPage(Data setPage)
    {
        page = setPage;
        setLables(setPage.Name);
        turnToPage();
    }

    private void setLables(string lable)
    {
            exteriorLabel.text = lable;
            interiorLabel.text = lable;
    }

    public void checkIfPage(Data currentPage)
    {
        if(page == null || page != currentPage)
        {
            turnFromPage();
        }
        else
        {
            turnToPage();
        }
    }

    public void removeFromPage()
    {
        page = null;
        setLables("<size=8>Click to assign open entry</size>");
        turnFromPage();
    }

    public void useBookmark()
    {
        if(page != null)
        {
            switch(page)
            {
                case TitleData:
                    notebookHandler.goToTitlePage();
                    break;
                case IndexData index:
                    notebookHandler.goToIndexPage(index);
                    break;
                case GenericData entry:
                    notebookHandler.goToEntryPage(entry);
                    break;
                default: 
                    break;
            }
        }
        else if(notebookHandler.onEntryPage())
        {
            setInPage(notebookHandler.entryPage.currentEntry);
        }
    }
}
