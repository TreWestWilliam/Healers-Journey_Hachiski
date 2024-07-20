using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class NotebookHandler : MonoBehaviour
{
    public ReputationUIHandler repHandler;
    public Records records;

    public EntryTile entryTilePrefab;
    public EntryTile unknownEntryTilePrefab;

    public CureTile cureRecipePrefab;
    public GameObject combineSymbolePrefab;
    public GameObject coldTemperaturePrefab;
    public GameObject defaultTemperaturePrefab;
    public GameObject hotTemperaturePrefab;

    public TMP_Text locationFoundPrefab;
    public TMP_Text unknownLocationPrefab;

    public TitlePage titlePage;
    public IndexPage indexPage;
    public EntryPage entryPage;

    public ColorRef pageColor;
    public ColorRef textColor;
    public ColorRef textColorDisabled;

    public Bookmark[] bookmarks;

    private GameObject currentPage;

    public void openNotebook()
    {
        if(!gameObject.activeInHierarchy)
        {
            open();
        }
    }

    private void open()
    {
        gameObject.SetActive(true);
    }

    public void closeNotebook()
    {
        if(gameObject.activeInHierarchy)
        {
            close();
        }
    }

    public void toggleNotebook()
    {
        if(gameObject.activeInHierarchy)
        {
            close();
        }
        else
        {
            open();
        }
    }

    private void close()
    {
        gameObject.SetActive(false);
    }

    public bool checkIfKnown(GenericData entry)
    {
        if(entry.tier > repHandler.rep.highestTierReached)
        {
            return false;
        }
        return true;
    }

    private void closeCurretPage()
    {
        if(currentPage != null)
        {
            currentPage.SetActive(false);
            currentPage = null;
        }
    }

    private void checkBookmarks()
    {
        foreach(Bookmark bookmark in bookmarks)
        {
            bookmark.checkIfPage(currentPageData());
        }
    }

    private void goToPage(GameObject page)
    {
        if(currentPage != page)
        {
            closeCurretPage();
            currentPage = page;
            currentPage.SetActive(true);
        }
        checkBookmarks();
    }

    public void goToTitlePage()
    {
        goToPage(titlePage.gameObject);
    }

    public void goToEntryPage(GenericData entry)
    {
        entryPage.openEntry(entry);
        goToPage(entryPage.gameObject);
    }

    public void goToIndexPage(IndexData index)
    {
        indexPage.openIndex(index);
        goToPage(indexPage.gameObject);
    }

    public bool onEntryPage()
    {
        return currentPage == entryPage.gameObject;
    }

    public void createEntryTile(GenericData data, Transform parent)
    {
        EntryTile newTile = Instantiate(entryTilePrefab, parent);
        newTile.gameObject.name = data.Name + " Tile";
        newTile.notebookHandler = this;
        newTile.setData(data);
    }

    public void createUnknownEntryTile(Transform parent)
    {
        EntryTile newTile = Instantiate(unknownEntryTilePrefab, parent);
        newTile.notebookHandler = this;
    }

    public void createCureRecipeTiles(Cure cure, Transform parent)
    {
        CureTile cureTile = Instantiate(cureRecipePrefab, parent);
        cureTile.setData(cure);
        cureTile.name = cure.cureName + " Tile";
        for(int i = 0; i < cure.recipe.Length; i++)
        {
            switch(cure.recipe[i].temperature)
            {
                case Temperature.Cold:
                    Instantiate(coldTemperaturePrefab, cureTile.transform);
                    break;
                case Temperature.Default:
                    Instantiate(defaultTemperaturePrefab, cureTile.transform);
                    break;
                case Temperature.Hot:
                    Instantiate(hotTemperaturePrefab, cureTile.transform);
                    break;
                default:
                    break;
            }
            createEntryTile(cure.recipe[i].ingredient, cureTile.transform);

            if(i < cure.recipe.Length - 1)
            {
                Instantiate(combineSymbolePrefab, cureTile.transform);
            }
        }

    }

    public void createLocationFound(string location, Transform parent)
    {
        TMP_Text Location = Instantiate(locationFoundPrefab, parent);

        Location.text += location;
    }

    public void createUnknownLocation(Transform parent)
    {
        TMP_Text Location = Instantiate(unknownLocationPrefab, parent);
    }

    public void clearSection(GameObject section)
    {
        foreach(Transform child in section.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private Data currentPageData()
    {
        if(currentPage == null)
        {
            return null;
        }
        else if(currentPage == titlePage.gameObject)
        {
            return titlePage.titleData;
        }
        else if(currentPage == indexPage.gameObject)
        {
            return indexPage.index;
        }
        else if(currentPage == entryPage.gameObject)
        {
            return entryPage.currentEntry;
        }
        return null;
    }

    private bool isCurrentPage(Data data)
    {
        return currentPageData() == data;
    }
}
