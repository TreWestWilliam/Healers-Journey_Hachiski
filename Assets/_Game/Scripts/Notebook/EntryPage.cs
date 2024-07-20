using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EntryPage : MonoBehaviour
{
    public GenericData currentEntry;

    public NotebookHandler notebookHandler;

    public Image page;

    public TMP_Text title;
    public TMP_Text tier;
    public Image icon;

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

    public Transform SymptomsA;
    public Transform SymptomsI;
    public Transform AilmentsI;
    public Transform AilmentsS;
    public Transform Ingredients;
    public Transform Cures;
    public Transform Locations;

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
                notebookHandler.createEntryTile(ailment.symptoms[i], SymptomsA);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(SymptomsA);
            }
        }

        for(int i = 0; i < ailment.cures.Length; i++)
        {
            if(checkDiscoveries(discoveries.cures, i))
            {
                notebookHandler.createCureRecipeTiles(ailment.cures[i], Cures);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(Cures);
            }
        }
    }

    private void openEntry(IngredientData ingredient, Discoveries discoveries)
    {
        IngredientDetails.SetActive(true);

        for(int i = 0; i < ingredient.symptoms.Length; i++)
        {
            if(checkDiscoveries(discoveries.symptoms, i))
            {
                notebookHandler.createEntryTile(ingredient.symptoms[i], SymptomsI);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(SymptomsI);
            }
        }

        for(int i = 0; i < ingredient.ailments.Length; i++)
        {
            if(checkDiscoveries(discoveries.ailments, i))
            {
                notebookHandler.createEntryTile(ingredient.ailments[i], AilmentsI);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(AilmentsI);
            }
        }

        for(int i = 0; i < ingredient.locationsFound.Length; i++)
        {
            if(checkDiscoveries(discoveries.locationsFound, i))
            {
                notebookHandler.createLocationFound(ingredient.locationsFound[i], Locations);
            }
            else
            {
                notebookHandler.createUnknownLocation(Locations);
            }
        }
    }

    private void openEntry(SymptomData symptom, Discoveries discoveries)
    {
        SymptomDetails.SetActive(true);

        for(int i = 0; i < symptom.ingredients.Length; i++)
        {
            if(checkDiscoveries(discoveries.symptoms, i))
            {
                notebookHandler.createEntryTile(symptom.ingredients[i], Ingredients);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(Ingredients);
            }
        }

        for(int i = 0; i < symptom.ailments.Length; i++)
        {
            if(checkDiscoveries(discoveries.ailments, i))
            {
                notebookHandler.createEntryTile(symptom.ailments[i], AilmentsS);
            }
            else
            {
                notebookHandler.createUnknownEntryTile(AilmentsS);
            }
        }
    }

    private bool checkDiscoveries(bool[] list, int index)
    {
        return !(list.Length <= index || !list[index]);
    }

}

