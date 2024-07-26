using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;

public class EntryPage : MonoBehaviour
{
    public GenericData currentEntry;

    public NotebookHandler notebookHandler;

    public Image page;

    public TMP_Text title;
    public TMP_Text tier;
    public SVGImage icon;

    public TMP_Text description;
    public TMP_Text unlockedText;


    public FontRef titleFont;
    public ColorRef titleColor;

    public FontRef descriptionFont;
    public ColorRef descriptionColor;

    public FontRef unlockedFont;
    public ColorRef unlockedColor;

    public GameObject AilmentDetails;
    public GameObject IngredientDetails;
    public GameObject SymptomDetails;

    public GridLayoutGroup SymptomsA;
    public GridLayoutGroup SymptomsI;
    public GridLayoutGroup AilmentsI;
    public GridLayoutGroup AilmentsS;
    public GridLayoutGroup Ingredients;
    public VerticalLayoutGroup Cures;
    public VerticalLayoutGroup Locations;

    // Start is called before the first frame update
    private void Start()
    {
        title.font = titleFont;
        title.color = titleColor;

        tier.font = titleFont;
        tier.color = titleColor;

        description.font = descriptionFont;
        description.color = descriptionColor;

        unlockedText.font = unlockedFont;
        unlockedText.color = unlockedColor;
    }

    private void clearPage()
    {
        AilmentDetails.SetActive(false);
        IngredientDetails.SetActive(false);
        SymptomDetails.SetActive(false);

        notebookHandler.clearSection(SymptomsA.gameObject);
        notebookHandler.clearSection(SymptomsI.gameObject);
        notebookHandler.clearSection(AilmentsI.gameObject);
        notebookHandler.clearSection(AilmentsS.gameObject);
        notebookHandler.clearSection(Ingredients.gameObject);
        notebookHandler.clearSection(Cures.gameObject);
        notebookHandler.clearSection(Locations.gameObject);
    }

    public void openEntry(GenericData chosenEntry)
    {
        clearPage();
        currentEntry = chosenEntry;

        Discoveries discoveries = notebookHandler.records.getDiscovery(currentEntry);
        discoveries ??= new Discoveries();

        title.text = currentEntry.Name;

        tier.text = notebookHandler.repHandler.tierNames[currentEntry.tier];

        icon.sprite = currentEntry.icon;

        description.text = currentEntry.Description;

        unlockedText.text = currentEntry.UnlockedDescription;
        if(discoveries.UnlockDescription)
        {
            unlockedText.gameObject.SetActive(true);
        }
        else
        {
            unlockedText.gameObject.SetActive(false);
        }
        
        switch(chosenEntry)
        {
            case AilmentData ailment:
                openEntry(ailment, discoveries);
                break;
            case IngredientData ingredient:
                openEntry(ingredient, discoveries);
                break;
            case SymptomData symptom:
                openEntry(symptom, discoveries);
                break;
            default:
                break;
        }
    }

    private void openEntry(AilmentData ailment, Discoveries discoveries)
    {
        AilmentDetails.SetActive(true);

        for(int i = 0; i < ailment.symptoms.Length; i++)
        {
            if(checkDiscoveries(discoveries.symptoms, i))
            {
                notebookHandler.createEntryTile(ailment.symptoms[i], SymptomsA.transform);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(SymptomsA.transform);
            }
        }
        if(ailment.symptoms.Length > 0)
        {
            StartCoroutine(notebookHandler.updateGrid(SymptomsA, ailment.symptoms.Length));
        }

        for(int i = 0; i < ailment.cures.Length; i++)
        {
            if(checkDiscoveries(discoveries.cures, i))
            {
                notebookHandler.createCureRecipeTiles(ailment.cures[i], Cures.transform);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(Cures.transform);
            }
        }
        if(ailment.cures.Length > 0)
        {
            StartCoroutine(notebookHandler.updateVertical(Cures, ailment.cures.Length));
        }
    }

    private void openEntry(IngredientData ingredient, Discoveries discoveries)
    {
        IngredientDetails.SetActive(true);

        for(int i = 0; i < ingredient.symptoms.Length; i++)
        {
            if(checkDiscoveries(discoveries.symptoms, i))
            {
                notebookHandler.createEntryTile(ingredient.symptoms[i], SymptomsI.transform);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(SymptomsI.transform);
            }
        }
        if(ingredient.symptoms.Length > 0)
        {
            StartCoroutine(notebookHandler.updateGrid(SymptomsI, ingredient.symptoms.Length));
        }

        for(int i = 0; i < ingredient.ailments.Length; i++)
        {
            if(checkDiscoveries(discoveries.ailments, i))
            {
                notebookHandler.createEntryTile(ingredient.ailments[i], AilmentsI.transform);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(AilmentsI.transform);
            }
        }
        if(ingredient.ailments.Length > 0)
        {
            StartCoroutine(notebookHandler.updateGrid(AilmentsI, ingredient.ailments.Length));
        }

        for(int i = 0; i < ingredient.locationsFound.Length; i++)
        {
            if(checkDiscoveries(discoveries.locationsFound, i))
            {
                notebookHandler.createLocationFound(ingredient.locationsFound[i], Locations.transform);
            }
            else
            {
                notebookHandler.createUnknownLocation(Locations.transform);
            }
        }
        if(ingredient.locationsFound.Length > 0)
        {
            StartCoroutine(notebookHandler.updateVertical(Locations, ingredient.locationsFound.Length));
        }
    }

    private void openEntry(SymptomData symptom, Discoveries discoveries)
    {
        SymptomDetails.SetActive(true);

        for(int i = 0; i < symptom.ingredients.Length; i++)
        {
            if(checkDiscoveries(discoveries.symptoms, i))
            {
                notebookHandler.createEntryTile(symptom.ingredients[i], Ingredients.transform);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(Ingredients.transform);
            }
        }
        if(symptom.ingredients.Length > 0)
        {
            StartCoroutine(notebookHandler.updateGrid(Ingredients, symptom.ingredients.Length));
        }

        for(int i = 0; i < symptom.ailments.Length; i++)
        {
            if(checkDiscoveries(discoveries.ailments, i))
            {
                notebookHandler.createEntryTile(symptom.ailments[i], AilmentsS.transform);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(AilmentsS.transform);
            }
        }
        if(symptom.ailments.Length > 0)
        {
            StartCoroutine(notebookHandler.updateGrid(AilmentsS, symptom.ailments.Length));
        }
    }

    private bool checkDiscoveries(bool[] list, int index)
    {
        return !(list.Length <= index || !list[index]);
    }

    public void discoverCuresDEBUG()
    {
        if(currentEntry is AilmentData)
        {
            for(int i = 0; i < (currentEntry as AilmentData).cures.Length; i++)
            {
                notebookHandler.records.discoverCure((currentEntry as AilmentData), i);
            }
        }
        openEntry(currentEntry);
    }
}
