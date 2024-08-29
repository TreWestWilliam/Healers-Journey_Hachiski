using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VectorGraphics;

public class PuzzleHandler : MonoBehaviour
{
    [SerializeField] private GameObject puzzleUI;
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private Reputation reputation;
    [SerializeField] private Records records;
    [SerializeField] private PlayerMovement player;
    //[SerializeField] private
    public NPC npc;

    [SerializeField] private TMP_Text npcName;
    [SerializeField] private TMP_Text dialogue;

    [SerializeField] private RecipeIngredientSlot[] ingredientSlots;

    public SpriteRef coldSprite;
    public SpriteRef defaultSprite;
    public SpriteRef hotSprite;

    public ColorRef coldColor;
    public ColorRef defaultColor;
    public ColorRef hotColor;

    [SerializeField] private int[] repPerCureByTier;
    [SerializeField] private int[] repPerFailByTier;
    [SerializeField] private GameObject feedbackKey;

    private TreatedIngredient[] recipe = {
        new TreatedIngredient(null, Temperature.Default),
        new TreatedIngredient(null, Temperature.Default),
        new TreatedIngredient(null, Temperature.Default) };

    [SerializeField] private GridLayoutGroup itemGrid;
    [SerializeField] private PuzzleInventorySlot inventorySlotPrefab;
    private Dictionary<IngredientData, PuzzleInventorySlot> inventorySlots = new Dictionary<IngredientData, PuzzleInventorySlot>();

    public void openInteraction(NPC npc)
    {
        this.npc = npc;
        records.discoverAilmentSymptomsIngredients(npc.ailment);

        npcName.text = npc.npcName + ":";
        try
        {
            dialogue.text = npc.ailment.complaints[0];
        }
        catch (IndexOutOfRangeException IO) 
        {
            Debug.LogWarning($"NPC ailment lacks complaint, giving default.  {IO.Message}",npc.gameObject);
            dialogue.text = "I feel really sick!";
        }
        

        /*for(int i = 0; i < npc.ailment.cures[0].recipe.Length; i++)
        {
            setIngredient(i, npc.ailment.cures[0].recipe[i].ingredient);
        }*/

        //Settings the correct number of slots for the cures available
        //This is only here incase multiple solutions are avaiable in the future.
        int highestLength = 0;
        if (npc.ailment.cures.Length == 0) { Debug.Log("There are no cures in the ailment", this); }
        foreach (Cure c in npc.ailment.cures) 
        {
            highestLength = c.recipe.Length > highestLength ? c.recipe.Length: highestLength;
        }
        // Incase something bad happens and we dont have the UI slots to fit the max 
        if (highestLength > ingredientSlots.Length) { highestLength = ingredientSlots.Length; }

        for (int i =0;i < ingredientSlots.Length;i++) 
        {
            try
            {
                ingredientSlots[i].gameObject.SetActive((i < highestLength));
            }
            catch (Exception e) {
                Debug.LogError($"Erorr: {e.Message}", this);
            }
            
        }


        createPuzzleInventory();

        puzzleUI.SetActive(true);
    }

    public void closeInteraction()
    {
        for(int i = 0; i < recipe.Length; i++)
        {
            clearTemp(i);
            removeIngredient(i);
            ingredientSlots[i].clearFeedback();
        }
        clearPuzzleInventory();
        feedbackKey.SetActive(false);
        puzzleUI.SetActive(false);
        npc.Disengage(player);
        npc = null;
    }

    #region Temperature
    public void setTemperature(int index, Temperature temperature)
    {
        if(index >= 0 && index < recipe.Length)
        {
            recipe[index].temperature = temperature;
            ingredientSlots[index].setSlotContents(recipe[index]);
            ingredientSlots[index].updateTempUI();
        }
    }

    private void clearTemp(int index)
    {
        setTemperature(index, Temperature.Default);
    }
    #endregion

    #region Ingredient
    public void setIngredient(int index, IngredientData ingredient)
    {
        if(index >= 0 && index < recipe.Length)
        {
            if(recipe[index].ingredient != null)
            {
                inventory.AddItem(recipe[index].ingredient, 1);
                updateItemQuantity(recipe[index].ingredient);
            }
            if(ingredient == null)
            {
                recipe[index].ingredient = null;
                updateIngredientUI(index);
            }
            else if(inventory.RemoveItem(ingredient, 1))
            {
                updateItemQuantity(ingredient);
                recipe[index].ingredient = ingredient;
                updateIngredientUI(index);
            }
            else
            {
                recipe[index].ingredient = null;
                updateIngredientUI(index);
            }
        }
    }

    private void removeIngredient(int index)
    {
        setIngredient(index, null);
    }
    #endregion

    private void emptySlot(int index)
    {
        if(index >= 0 && index < recipe.Length)
        {
            clearTemp(index);
            recipe[index].ingredient = null;
            updateIngredientUI(index);
        }
    }

    private void updateIngredientUI(int index)
    {
        ingredientSlots[index].setSlotContents(recipe[index]);
        ingredientSlots[index].updateIngredientUI();
    }

    private void emptyAndRefillSlot(int index)
    {
        if(index >= 0 && index < recipe.Length)
        {
            TreatedIngredient refill = new TreatedIngredient(recipe[index].ingredient, recipe[index].temperature);
            emptySlot(index);
            setTemperature(index, refill.temperature);
            setIngredient(index, refill.ingredient);
        }
    }

    private void createPuzzleInventory()
    {
        int count = 0;
        RectTransform rectTransform = itemGrid.transform as RectTransform;
        foreach(ItemQuantity iq in inventory.items)
        {
            IngredientData ingredient = iq.item as IngredientData;
            if(ingredient != null && !inventorySlots.ContainsKey(ingredient))
            {
                PuzzleInventorySlot newSlot = Instantiate<PuzzleInventorySlot>(inventorySlotPrefab, itemGrid.transform);
                newSlot.setUpSlot(iq.item, iq.quantity);
                inventorySlots.Add(ingredient, newSlot);
                count++;
            }
        }

        rectTransform.anchoredPosition = Vector2.zero;
        int rows = (int)MathF.Ceiling((float)count / 3f);
        float height = ((itemGrid.cellSize.y + itemGrid.spacing.y) * (float)rows) + itemGrid.padding.top + itemGrid.padding.bottom - itemGrid.spacing.y;
        float width = rectTransform.sizeDelta.x;
        rectTransform.sizeDelta = new Vector2(width, height);
    }

    private void updateItemQuantity(IngredientData ingredient)
    {
        if(inventorySlots.ContainsKey(ingredient))
        {
            inventorySlots[ingredient].updateQuantity(inventory.getQuantity(ingredient));
        }
    }

    private void clearPuzzleInventory()
    {
        inventorySlots.Clear();
        foreach(Transform child in itemGrid.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private bool compareRecipes(TreatedIngredient[] cureRecipe)
    {
        bool result = true;
        /* This will always return false for smaller recipes, since recipe.Length is always the total amount of ingredient slots we could use.
         * This doesn't seem to have been preventing any issues anyhow, but left it only commented out just in case.
        if(recipe.Length != cureRecipe.Length)
        {
            return false;
        }
        */
        for(int i = 0; i < cureRecipe.Length; i++)
        {
            if(recipe[i] != cureRecipe[i])
            {
                result = false;
                if(recipe[i].temperature == cureRecipe[i].temperature ||
                    checkAgainstIngredients(cureRecipe, recipe[i].ingredient))
                {
                    ingredientSlots[i].displayFeedback(1);
                }
                else
                {
                    ingredientSlots[i].displayFeedback(0);
                }
            }
            else
            {
                ingredientSlots[i].displayFeedback(2);
            }
        }
        return result;
    }

    private bool checkAgainstIngredients(TreatedIngredient[] cureRecipe, IngredientData ingredient)
    {
        for(int i = 0; i < cureRecipe.Length; i++)
        {
            if(recipe[i] != cureRecipe[i] && cureRecipe[i].ingredient == ingredient)
            {
                return true;
            }
        }
        return false;
    }

    public void createCure()
    {
        if(npc.ailment != null)
        {
            int tier = npc.ailment.tier;
            if(tier > reputation.RepTier)
            {
                dialogue.text = "I don't trust you to treat this right now.";
            }
            else
            {
                if(compareRecipes(npc.ailment.cures[0].recipe))
                {
                    if(tier >= repPerCureByTier.Length)
                    {
                        tier = repPerCureByTier.Length - 1;
                    }
                    if(tier >= 0)
                    {
                        reputation.adjustRep(repPerCureByTier[tier]);
                    }

                    for(int i = 0; i < recipe.Length; i++)
                    {
                        ingredientSlots[i].clearFeedback();
                        emptySlot(i);
                    }
                    records.discoverCure(npc.ailment, 0);
                    feedbackKey.SetActive(false);

                    dialogue.text = "Thank you!  That worked!";
                    npc.recieveCure();
                }
                else
                {
                    if(tier >= repPerFailByTier.Length)
                    {
                        tier = repPerFailByTier.Length - 1;
                    }
                    if(tier >= 0)
                    {
                        reputation.adjustRep(repPerFailByTier[tier]);
                    }

                    for(int i = 0; i < recipe.Length; i++)
                    {
                        emptyAndRefillSlot(i);
                    }

                    feedbackKey.SetActive(true);

                    dialogue.text = "That didn't help!";
                }
            }
        }
    }
}
