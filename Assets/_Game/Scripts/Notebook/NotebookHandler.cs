using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Linq;

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

    public ColorRef iconColorDisabled;
    public Material iconMaterialDisabled;

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
        if(currentPage == indexPage.gameObject)
        {
            goToIndexPage(indexPage.index);
        }
        else if(currentPage == entryPage.gameObject)
        {
            goToEntryPage(entryPage.currentEntry);
        }
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
        /*if(entry.tier > repHandler.rep.highestTierReached)
        {
            return false;
        }
        return true;*/
        return records.topicDiscovered(entry);
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
    }

    public void goToTitlePage()
    {
        goToPage(titlePage.gameObject);
        checkBookmarks();
    }

    public void goToEntryPage(GenericData entry)
    {
        goToPage(entryPage.gameObject);
        entryPage.openEntry(entry);
        checkBookmarks();
    }

    public void goToIndexPage(IndexData index)
    {
        goToPage(indexPage.gameObject);
        indexPage.openIndex(index);
        checkBookmarks();
    }

    public bool onEntryPage()
    {
        return currentPage == entryPage.gameObject;
    }

    public EntryTile createEntryTile(GenericData data, Transform parent)
    {
        EntryTile newTile = Instantiate(entryTilePrefab, parent);
        newTile.gameObject.name = data.Name + " Tile";
        newTile.notebookHandler = this;
        newTile.setData(data);
        return newTile;
    }

    public EntryTile createUnknownEntryTile(Transform parent)
    {
        EntryTile newTile = Instantiate(unknownEntryTilePrefab, parent);
        newTile.notebookHandler = this;
        return newTile;
    }

    public CureTile createCureRecipeTiles(Cure cure, Transform parent)
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
        return cureTile;
    }

    public TMP_Text createLocationFound(string location, Transform parent)
    {
        TMP_Text Location = Instantiate(locationFoundPrefab, parent);

        Location.text += location;

        return Location;
    }

    public TMP_Text createUnknownLocation(Transform parent)
    {
        TMP_Text Location = Instantiate(unknownLocationPrefab, parent);

        return Location;
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

    public IEnumerator updateGrid(GridLayoutGroup grid, int count)
    {
        yield return new WaitForFixedUpdate();

        RectTransform gridTransform = grid.transform as RectTransform;
        GridLayoutGroup eG = grid;

        int columns = Mathf.FloorToInt((gridTransform.rect.width - eG.padding.left - eG.padding.right + eG.spacing.x) / (eG.cellSize.x + eG.spacing.x));

        int rows = Mathf.CeilToInt((float)(count) / (float)columns);

        float height = ((eG.cellSize.y + eG.spacing.y) * (float)rows) + eG.padding.top + eG.padding.bottom - eG.spacing.y;
        float width = gridTransform.sizeDelta.x;
        if(height > gridTransform.rect.height)
        {
            gridTransform.sizeDelta = new Vector2(width, height - gridTransform.rect.height);
        }
    }

    public IEnumerator updateVertical(VerticalLayoutGroup vertical, int count)
    {
        yield return new WaitForFixedUpdate();

        RectTransform verticalTransform = vertical.transform as RectTransform;
        VerticalLayoutGroup vG = vertical;

        float childHeight = 0f;

        foreach(Transform child in vertical.transform)
        {
            childHeight += (child as RectTransform).rect.height;
        }

        float height = vG.padding.top + childHeight + (vG.spacing * (float)count - 1) + vG.padding.bottom;
        float width = verticalTransform.sizeDelta.x;
        if(height > verticalTransform.rect.height)
        {
            verticalTransform.sizeDelta = new Vector2(width, height - verticalTransform.rect.height);
        }
    }

#if UNITY_EDITOR
    public void DiscoverTier1IngredientsAndSymptoms()
    {
        foreach(Bookmark bookmark in bookmarks)
        {
            switch(bookmark.page)
            {
                case IngredientIndex index:
                    if(index.tiers != null && index.tiers.Length > 0 &&
                        index.tiers[0] != null && index.tiers[0].data != null
                        && index.tiers[0].data.Length > 0)
                    {
                        foreach(GenericData data in index.tiers[0].data)
                        {
                            if(data is IngredientData)
                            {
                                records.discoverIngredientSymptoms(data as IngredientData);
                            }
                        }
                    }
                    break;
                case SymptomIndex index:
                    if(index.tiers != null && index.tiers.Length > 0 &&
                        index.tiers[0] != null && index.tiers[0].data != null
                        && index.tiers[0].data.Length > 0)
                    {
                        foreach(GenericData data in index.tiers[0].data)
                        {
                            if(data is SymptomData)
                            {
                                records.discoverSymptomIngredients(data as SymptomData);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void DiscoverTier1Ailments()
    {
        foreach(Bookmark bookmark in bookmarks)
        {
            switch(bookmark.page)
            {
                case AilmentIndex index:
                    if(index.tiers != null && index.tiers.Length > 0 &&
                        index.tiers[0] != null && index.tiers[0].data != null
                        && index.tiers[0].data.Length > 0)
                    {
                        foreach(GenericData data in index.tiers[0].data)
                        {
                            if(data is AilmentData)
                            {
                                records.discover(data);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void DiscoverTier1AilmentSymptoms()
    {
        foreach(Bookmark bookmark in bookmarks)
        {
            switch(bookmark.page)
            {
                case AilmentIndex index:
                    if(index.tiers != null && index.tiers.Length > 0 &&
                        index.tiers[0] != null && index.tiers[0].data != null
                        && index.tiers[0].data.Length > 0)
                    {
                        foreach(GenericData data in index.tiers[0].data)
                        {
                            if(data is AilmentData)
                            {
                                records.discoverAilmentSymptoms(data as AilmentData);
                            }
                        }
                    }
                    break;
                default:
                    break;
            }
        }
    }
#endif
}
