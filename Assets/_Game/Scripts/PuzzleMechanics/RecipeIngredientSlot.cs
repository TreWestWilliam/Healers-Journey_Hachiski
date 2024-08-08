using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RecipeIngredientSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] private PuzzleHandler puzzleHandler;
    [SerializeField] private int index = 0;

    [SerializeField] private SVGImage tempImage;
    [SerializeField] private Slider tempSlider;
    [SerializeField] private SVGImage ingredientImage;
    [SerializeField] private TMP_Text ingredientName;

    [SerializeField] private Button clearIngredientButton;

    [SerializeField] private GameObject[] feedback;

    [SerializeField] private ItemDragger itemDragger;

    private TreatedIngredient treatedIngredient = new TreatedIngredient(null, Temperature.Default);

    private void Awake()
    {
        updateTempUI();
        updateIngredientUI();
    }

    public void setSlotContents(TreatedIngredient contents)
    {
        treatedIngredient.ingredient = contents.ingredient;
        treatedIngredient.temperature = contents.temperature;
        itemDragger.setItem(treatedIngredient.ingredient);

        if((int)tempSlider.value != (int)contents.temperature)
        {
            tempSlider.value = (int)contents.temperature;
        }
        if(treatedIngredient.ingredient == null)
        {
            clearIngredientButton.gameObject.SetActive(false);
        }
        else
        {
            clearIngredientButton.gameObject.SetActive(true);
        }
    }

    public void updateTempUI()
    {
        switch(treatedIngredient.temperature)
        {
            case Temperature.Cold:
                tempImage.sprite = puzzleHandler.coldSprite;
                tempImage.color = puzzleHandler.coldColor;
                break;
            case Temperature.Hot:
                tempImage.sprite = puzzleHandler.hotSprite;
                tempImage.color = puzzleHandler.hotColor;
                break;
            default:
                tempImage.sprite = puzzleHandler.defaultSprite;
                tempImage.color = puzzleHandler.defaultColor;
                break;
        }
    }

    public void updateIngredientUI()
    {
        if(treatedIngredient.ingredient == null)
        {
            ingredientImage.sprite = null;
            ingredientImage.color = Color.clear;
            ingredientName.text = "";
        }
        else
        {
            ingredientImage.sprite = treatedIngredient.ingredient.icon;
            ingredientImage.color = Color.white;
            ingredientName.text = treatedIngredient.ingredient.Name;
        }
    }

    public void clearFeedback()
    {
        foreach(GameObject option in feedback)
        {
            option.SetActive(false);
        }
    }

    public void displayFeedback(int option)
    {
        clearFeedback();
        if(option >= 0 && option < feedback.Length)
        {
            feedback[option].gameObject.SetActive(true);
        }
    }

    public void setTemperature(float temperature)
    {
        Temperature temp = default(Temperature);
        switch(temperature)
        {
            case -1:
                temp = Temperature.Cold;
                break;
            case 1:
                temp = Temperature.Hot;
                break;
            default:
                break;
        }

        puzzleHandler.setTemperature(index, temp);
    }

    private void setIngredient(IngredientData ingredient)
    {
        puzzleHandler.setIngredient(index, ingredient);
    }

    public void clearIngredient()
    {
        puzzleHandler.setIngredient(index, null);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        if(dropped != itemDragger.gameObject)
        {
            ItemDragger dragger = dropped.GetComponent<ItemDragger>();
            if(dragger != null)
            {
                IngredientData ingredient = dragger.getItem() as IngredientData;
                if(ingredient != null)
                {
                    dragger.clearSlot();
                    setIngredient(ingredient);
                }
            }
        }
    }
}
