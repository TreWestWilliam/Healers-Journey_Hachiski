using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public enum Temperature
{
    Cold = -1,
    Default = 0,
    Hot = 1
}

[Serializable]
public class TreatedIngredient : IEquatable<TreatedIngredient>
{
    public IngredientData ingredient;
    public Temperature temperature;

    public static bool operator ==(TreatedIngredient v1, TreatedIngredient v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return false;
        }
        return v1.Equals(v2);
    }

    public static bool operator !=(TreatedIngredient v1, TreatedIngredient v2)
    {
        return !(v1 == v2);
    }

    public bool Equals(TreatedIngredient other)
    {
        if(other == null) return false;

        return ingredient.Equals(other.ingredient) && temperature.Equals(other.temperature);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(ingredient, temperature);
    }

    public override bool Equals(object obj)
    {
        return obj is TreatedIngredient @ref && this.Equals(@ref);
    }
}


[Serializable]
public class Cure : IComparable<Cure>
{
    public StringRef cureName;

    public SpriteRef cureSprite;

    public StringRef preparation;

    public TreatedIngredient[] recipe;

    public int tier => getTier();

    public bool containsIngredient(IngredientData ingredient)
    {
        IngredientData[] ingredients = { ingredient };
        return containsAnyOfIngredients(ingredients);
    }

    public bool containsAnyOfIngredients(IngredientData[] ingredients)
    {
        foreach(TreatedIngredient tIngredient in recipe)
        {
            foreach(IngredientData ingredient in ingredients)
            {
                if(tIngredient.ingredient == ingredient)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private int getTier()
    {
        int tier = 0;
        foreach(TreatedIngredient ingredient in recipe)
        {
            if(ingredient.ingredient.tier > tier)
            {
                tier = ingredient.ingredient.tier;
            }
        }
        return tier;
    }

    public int CompareTo(Cure other)
    {
        if(other == null) return 1;

        int thisTier = tier;
        int otherTier = other.tier;
        if(thisTier != otherTier)
        {
            return thisTier.CompareTo(otherTier);
        }

        if(cureName.Value != other.cureName.Value)
        {
            return cureName.Value.CompareTo(other.cureName.Value);
        }

        return recipe.Length.CompareTo(other.recipe.Length);
    }
}

[CreateAssetMenu(fileName = "Ailment", menuName = "Data/Ailment", order = 0)]
public class AilmentData : GenericData
{
    public SymptomData[] symptoms = { };

    public Cure[] cures = { };

    public override DiscoveryTypeIndices getDataIndex(GenericData data)
    {
        switch(data)
        {
            case IngredientData ingredient:

                List<int> indices = new List<int>();
                for(int i = 0; i < cures.Length; i++)
                {
                    if(cures[i].containsIngredient(ingredient))
                    {
                        indices.Add(i);
                    }
                }
                if(indices.Count > 0)
                {
                    return new DiscoveryTypeIndices(DiscoveryType.Cure, indices.ToArray());
                }
                return null;
            case SymptomData symptom:
                for(int i = 0; i < symptoms.Length; i++)
                {
                    if(symptoms[i] == symptom)
                    {
                        int[] index = { i };
                        return new DiscoveryTypeIndices(DiscoveryType.Symptom, index);
                    }
                }
                return null;
            default:
                return null;
        }
    }

    public override DiscoveryTypeIndices[] getDataIndex(GenericData[] data)
    {
        List<IngredientData> ingredientData = new List<IngredientData>();
        List<SymptomData> symptomData = new List<SymptomData>();
        foreach(GenericData generic in data)
        {
            switch(generic)
            {
                case IngredientData ingredient:
                    ingredientData.Add(ingredient);
                    break;
                case SymptomData symptom:
                    symptomData.Add(symptom);
                    break;
                default:
                    break;
            }
        }

        List<DiscoveryTypeIndices> discoveryTypeIndices = new List<DiscoveryTypeIndices>();
        if(ingredientData.Count > 0)
        {
            List<int> indices = new List<int>();
            for(int i = 0; i < cures.Length; i++)
            {
                if(cures[i].containsAnyOfIngredients(ingredientData.ToArray()))
                {
                    indices.Add(i);
                }
            }
            if(indices.Count > 0)
            {
                discoveryTypeIndices.Add(new DiscoveryTypeIndices(DiscoveryType.Cure, indices.ToArray()));
            }
        }

        if(symptomData.Count > 0)
        {
            List<int> indices = new List<int>();
            for(int i = 0; i < symptoms.Length; i++)
            {
                foreach(SymptomData symptom in symptomData)
                {
                    if(symptoms[i] == symptom)
                    {
                        indices.Add(i);
                        break;
                    }
                }
            }
            if(indices.Count > 0)
            {
                discoveryTypeIndices.Add(new DiscoveryTypeIndices(DiscoveryType.Symptom, indices.ToArray()));
            }
        }

        if(discoveryTypeIndices.Count > 0)
        {
            return discoveryTypeIndices.ToArray();
        }

        return null;
    }

#if UNITY_EDITOR
    public override void sortArrays()
    {
        List<SymptomData> symptomList = new List<SymptomData>();
        List<Cure> cureList = new List<Cure>();

        foreach(SymptomData symptom in symptoms)
        {
            if(!symptomList.Contains(symptom))
            {
                symptomList.Add(symptom);
            }
        }

        foreach(Cure cure in cures)
        {
            if(!cureList.Contains(cure))
            {
                cureList.Add(cure);
            }
        }

        symptomList.Sort();
        cureList.Sort();

        symptoms = symptomList.ToArray();
        cures = cureList.ToArray();
    }

    public override void reciprocateData()
    {
        foreach(SymptomData symptom in symptoms)
        {
            symptom.ailments ??= new AilmentData[0];
            List<AilmentData> ailments = new List<AilmentData>(symptom.ailments);
            if(!ailments.Contains(this))
            {
                ailments.Add(this);
                ailments.Sort();
            }
            symptom.ailments = ailments.ToArray();
            if(symptom.tier > tier)
            {
                symptom.tier = tier;
            }
        }

        List<IngredientData> ingredients = new List<IngredientData>();
        cures ??= new Cure[0];
        foreach(Cure cure in cures)
        {
            cure.recipe ??= new TreatedIngredient[0];
            foreach(TreatedIngredient treatedIngredient in cure.recipe)
            {
                if(treatedIngredient.ingredient != null && !ingredients.Contains(treatedIngredient.ingredient))
                {
                    ingredients.Add(treatedIngredient.ingredient);
                }
            }
        }
        foreach(IngredientData ingredient in ingredients)
        {
            ingredient.ailments ??= new AilmentData[0];
            List<AilmentData> ailments = new List<AilmentData>(ingredient.ailments);
            if(!ailments.Contains(this))
            {
                ailments.Add(this);
                ailments.Sort();
            }
            ingredient.ailments = ailments.ToArray();
        }
    }
#endif
}
