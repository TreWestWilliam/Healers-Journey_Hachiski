using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Symptom", menuName = "Data/Symptom", order = 2)]
public class SymptomData : GenericData
{
    public AilmentData[] ailments = { };
    public IngredientData[] ingredients = { };

    public StringRef[] complaints = { };

    public override DiscoveryTypeIndices getDataIndex(GenericData data)
    {
        switch(data)
        {
            case AilmentData ailment:
                for(int i = 0; i < ailments.Length; i++)
                {
                    if(ailments[i] == ailment)
                    {
                        int[] index = { i };
                        return new DiscoveryTypeIndices(DiscoveryType.Ailment, index);
                    }
                }
                return null;
            case IngredientData ingredient:
                for(int i = 0; i < ingredients.Length; i++)
                {
                    if(ingredients[i] == ingredient)
                    {
                        int[] index = { i };
                        return new DiscoveryTypeIndices(DiscoveryType.Ingredient, index);
                    }
                }
                return null;
            default:
                return null;
        }
    }

    public override DiscoveryTypeIndices[] getDataIndex(GenericData[] data)
    {
        List<AilmentData> ailmentData = new List<AilmentData>();
        List<IngredientData> ingredientData = new List<IngredientData>();
        foreach(GenericData generic in data)
        {
            switch(generic)
            {
                case AilmentData ailment:
                    ailmentData.Add(ailment);
                    break;
                case IngredientData ingredient:
                    ingredientData.Add(ingredient);
                    break;
                default:
                    break;
            }
        }

        List<DiscoveryTypeIndices> discoveryTypeIndices = new List<DiscoveryTypeIndices>();
        if(ailmentData.Count > 0)
        {
            List<int> indices = new List<int>();
            for(int i = 0; i < ailments.Length; i++)
            {
                foreach(AilmentData ailment in ailmentData)
                {
                    if(ailments[i] == ailment)
                    {
                        indices.Add(i);
                        break;
                    }
                }
            }
            if(indices.Count > 0)
            {
                discoveryTypeIndices.Add(new DiscoveryTypeIndices(DiscoveryType.Ailment, indices.ToArray()));
            }
        }

        if(ingredientData.Count > 0)
        {
            List<int> indices = new List<int>();
            for(int i = 0; i < ingredients.Length; i++)
            {
                foreach(IngredientData ingredient in ingredientData)
                {
                    if(ingredients[i] == ingredient)
                    {
                        indices.Add(i);
                        break;
                    }
                }
            }
            if(indices.Count > 0)
            {
                discoveryTypeIndices.Add(new DiscoveryTypeIndices(DiscoveryType.Ingredient, indices.ToArray()));
            }
        }

        if(discoveryTypeIndices.Count > 0)
        {
            return discoveryTypeIndices.ToArray();
        }

        return null;
    }

    public string getComplaint()
    {
        return getComplaint(0);
    }

    public string getComplaint(int index)
    {
        if(complaints.Length == 0)
        {
            throw new System.ArgumentOutOfRangeException($"No complaints for {Name} symptom.");
        }

        if(index >= complaints.Length)
        {
            return complaints[0];
        }

        return complaints[index];
    }

#if UNITY_EDITOR
    public override void sortArrays()
    {
        List<AilmentData> ailmentList = new List<AilmentData>();
        List<IngredientData> ingredientList = new List<IngredientData>();

        foreach(AilmentData ailment in ailments)
        {
            if(!ailmentList.Contains(ailment))
            {
                ailmentList.Add(ailment);
            }
        }

        foreach(IngredientData ingredient in ingredients)
        {
            if(!ingredientList.Contains(ingredient))
            {
                ingredientList.Add(ingredient);
            }
        }

        ailmentList.Sort();
        ingredientList.Sort();

        ailments = ailmentList.ToArray();
        ingredients = ingredientList.ToArray();
    }

    public override void reciprocateData()
    {
        foreach(AilmentData ailment in ailments)
        {
            ailment.symptoms ??= new SymptomData[0];
            List<SymptomData> symptoms = new List<SymptomData>(ailment.symptoms);
            if(!symptoms.Contains(this))
            {
                symptoms.Add(this);
                symptoms.Sort();
            }
            ailment.symptoms = symptoms.ToArray();
            if(ailment.tier < tier)
            {
                tier = ailment.tier;
            }
        }

        foreach(IngredientData ingredient in ingredients)
        {
            ingredient.symptoms ??= new SymptomData[0];
            List<SymptomData> symptoms = new List<SymptomData>(ingredient.symptoms);
            if(!symptoms.Contains(this))
            {
                symptoms.Add(this);
                symptoms.Sort();
            }
            ingredient.symptoms = symptoms.ToArray();
        }
    }
#endif
}
