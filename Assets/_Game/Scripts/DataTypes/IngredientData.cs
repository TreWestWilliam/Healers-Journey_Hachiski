using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Data/Ingredient", order = 1)]
public class IngredientData : ItemData
{
    public AilmentData[] ailments = { };
    public SymptomData[] symptoms = { };

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
        List<AilmentData> ailmentData = new List<AilmentData>();
        List <SymptomData> symptomData = new List<SymptomData>();
        foreach(GenericData generic in data)
        {
            switch(generic)
            {
                case AilmentData ailment:
                    ailmentData.Add(ailment);
                    break;
                case SymptomData symptom:
                    symptomData.Add(symptom);
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
                discoveryTypeIndices.Add(new DiscoveryTypeIndices(DiscoveryType.Ailment,indices.ToArray()));
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
        List<AilmentData> ailmentList = new List<AilmentData>();
        List<SymptomData> symptomList = new List<SymptomData>();
        List<StringRef> locationList = new List<StringRef>();

        foreach(AilmentData ailment in ailments)
        {
            if(!ailmentList.Contains(ailment))
            {
                ailmentList.Add(ailment);
            }
        }

        foreach(SymptomData symptom in symptoms)
        {
            if(!symptomList.Contains(symptom))
            {
                symptomList.Add(symptom);
            }
        }

        foreach(StringRef location in locationsFound)
        {
            if(!locationList.Contains(location))
            {
                locationList.Add(location);
            }
        }

        ailmentList.Sort();
        symptomList.Sort();

        ailments = ailmentList.ToArray();
        symptoms = symptomList.ToArray();
        locationsFound = locationList.ToArray();
    }

    public override void reciprocateData()
    {
        foreach(SymptomData symptom in symptoms)
        {
            symptom.ingredients ??= new IngredientData[0];
            List<IngredientData> ingredients = new List<IngredientData>(symptom.ingredients);
            if(!ingredients.Contains(this))
            {
                ingredients.Add(this);
                ingredients.Sort();
            }
            symptom.ingredients = ingredients.ToArray();
        }
    }
#endif
}
