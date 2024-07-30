using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DiscoveryType
{
    Topic,
    Description,
    Ailment,
    Ingredient,
    Symptom,
    Cure,
    Location
}

[Serializable]
public class Discoveries
{
    public GenericData topic;
    public bool UnlockDescription = false;
    public bool[] ailments = { };
    public bool[] ingredients = { };
    public bool[] symptoms = { };
    public bool[] cures = { };
    public bool[] locationsFound = { };
}

public class DiscoveryTypeIndices
{
    public DiscoveryType discoveryType;
    public int[] indices;

    public DiscoveryTypeIndices(DiscoveryType discoveryType, int[] indices)
    {
        this.discoveryType = discoveryType;
        this.indices = indices;
    }
}

public class Records : MonoBehaviour
{
    [SerializeField]
    private List<Discoveries> discoveries;

    private void Awake()
    {
        discoveries ??= new List<Discoveries>();
    }

    public Discoveries getDiscovery(GenericData topic)
    {
        foreach(Discoveries Topic in discoveries)
        {
            if(Topic.topic == topic)
            {
                return Topic;
            }
        }
        return null;
    }

    public bool topicDiscovered(GenericData topic)
    {
        return getDiscovery(topic) != null;
    }

    public bool discovered(GenericData topic, DiscoveryType discoveryType, int index)
    {
        Discoveries discovery = getDiscovery(topic);

        return discovered(discovery, discoveryType, index);
    }

    public bool discovered(Discoveries discovery, DiscoveryType discoveryType, int index)
    {
        
        if(discovery == null || discovery.topic == null)
        {
            return false;
        }

        switch(discoveryType)
        {
            case DiscoveryType.Topic:
                break;
            case DiscoveryType.Description:
                return discovery.UnlockDescription;
            case DiscoveryType.Ailment:
                if(index < discovery.ailments.Length)
                {
                    return discovery.ailments[index];
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{discovery.topic.Name} does not have {index} Ailments.");
                }
            case DiscoveryType.Ingredient:
                if(index < discovery.ingredients.Length)
                {
                    return discovery.ingredients[index];
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{discovery.topic.Name} does not have {index} Ingredients.");
                }
            case DiscoveryType.Symptom:
                if(index < discovery.symptoms.Length)
                {
                    return discovery.symptoms[index];
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{discovery.topic.Name} does not have {index} Symptom.");
                }
            case DiscoveryType.Cure:
                if(index < discovery.cures.Length)
                {
                    return discovery.cures[index];
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{discovery.topic.Name} does not have {index} Cures.");
                }
            case DiscoveryType.Location:
                if(index < discovery.locationsFound.Length)
                {
                    return discovery.locationsFound[index];
                }
                else
                {
                    throw new System.ArgumentOutOfRangeException($"{discovery.topic.Name} is not found in {index} locations.");
                }
            default:
                throw new System.ArgumentOutOfRangeException($"That's impossible to know about {discovery.topic.Name}.");
        }
        return false;
    }

    private Discoveries discoverTopic(GenericData topic)
    {
        Discoveries newTopic = new Discoveries();
        newTopic.topic = topic;
        switch(topic)
        {
            case AilmentData ailment:
                newTopic.symptoms = new bool[ailment.symptoms.Length];
                newTopic.cures = new bool[ailment.cures.Length];
                break;
            case IngredientData ingredient:
                newTopic.ailments = new bool[ingredient.ailments.Length];
                newTopic.symptoms = new bool[ingredient.symptoms.Length];
                newTopic.locationsFound = new bool[ingredient.locationsFound.Length];
                break;
            case SymptomData symptom:
                newTopic.ailments = new bool[symptom.ailments.Length];
                newTopic.ingredients = new bool[symptom.ingredients.Length];
                break;
            default:
                break;
        }
        discoveries.Add(newTopic);

        return newTopic;
    }

    public void discoverIngredient(IngredientData ingredient, int locationIndex)
    {
        discover(ingredient, DiscoveryType.Location, locationIndex);
    }

    public void discoverSymptom(SymptomData symptom)
    {
        discover(symptom);
    }

    public void discoverAilment(AilmentData ailment)
    {
        discover(ailment);
    }

    public void discoverAilmentSymptoms(AilmentData ailment)
    {
        int[] symptomIndices = new int[ailment.symptoms.Length];
        for(int i = 0; i < symptomIndices.Length; i++)
        {
            symptomIndices[i] = i;

            DiscoveryTypeIndices ailmentIndex = ailment.symptoms[i].getDataIndex(ailment);
            if(ailmentIndex != null)
            {
                discover(ailment.symptoms[i], ailmentIndex);
            }
        }

        discover(ailment, new DiscoveryTypeIndices(DiscoveryType.Symptom, symptomIndices));
    }

    public void discoverIngredientSymptoms(IngredientData ingredient)
    {
        int[] symptomIndices = new int[ingredient.symptoms.Length];
        for(int i = 0; i < symptomIndices.Length; i++)
        {
            symptomIndices[i] = i;
        }

        discover(ingredient, new DiscoveryTypeIndices(DiscoveryType.Symptom, symptomIndices));
    }

    public void discoverSymptomIngredients(SymptomData symptom)
    {
        int[] ingredientIndices = new int[symptom.ingredients.Length];
        for(int i = 0; i < ingredientIndices.Length; i++)
        {
            ingredientIndices[i] = i;
        }

        discover(symptom, new DiscoveryTypeIndices(DiscoveryType.Ingredient, ingredientIndices));
    }

    public void discoverCure(AilmentData ailment, int cureIndex)
    {
        discover(ailment, DiscoveryType.Cure, cureIndex);
        List<IngredientData> ingredients = new List<IngredientData>();
        foreach(TreatedIngredient treatedIngredient in ailment.cures[cureIndex].recipe)
        {
            if(!ingredients.Contains(treatedIngredient.ingredient))
            {
                ingredients.Add(treatedIngredient.ingredient);
            }
        }

        foreach(SymptomData symptom in ailment.symptoms)
        {
            GenericData[] data = new GenericData[ingredients.Count + 1];
            data[0] = ailment;
            ingredients.ToArray().CopyTo(data, 1);

            DiscoveryTypeIndices[] indices = symptom.getDataIndex(data);
            if(indices != null)
            {
                discover(symptom, indices);
            }
        }
        foreach(IngredientData ingredient in ingredients)
        {
            GenericData[] data = new GenericData[ailment.symptoms.Length + 1];
            data[0] = ailment;
            ailment.symptoms.CopyTo(data, 1);

            DiscoveryTypeIndices[] indices = ingredient.getDataIndex(data);
            if(indices != null)
            {
                discover(ingredient, indices);
            }
        }
    }

    public void discover(GenericData topic)
    {
        discover(topic, new DiscoveryTypeIndices(DiscoveryType.Topic, new int[0]));
    }

    public void discover(GenericData topic, DiscoveryType detail)
    {
        int[] i = { 0 };
        discover(topic, new DiscoveryTypeIndices(detail, i));
    }

    public void discover(GenericData topic, DiscoveryType detail, int index)
    {
        int[] i = { index };
        discover(topic, new DiscoveryTypeIndices(detail, i));
    }

    public void discover(GenericData topic, DiscoveryTypeIndices details)
    {
        DiscoveryTypeIndices[] array = { details };
        discover(topic, array);
    }

    public void discover(GenericData topic, DiscoveryTypeIndices[] details)
    {
        Discoveries Topic = getDiscovery(topic);
        Topic ??= discoverTopic(topic);

        foreach(DiscoveryTypeIndices detail in details)
        {
            switch(detail.discoveryType)
            {
                case DiscoveryType.Topic:
                    break;
                case DiscoveryType.Description:
                    Topic.UnlockDescription = true;
                    break;
                case DiscoveryType.Ailment:
                    foreach(int i in detail.indices)
                    {
                        if(i < Topic.ailments.Length)
                        {
                            Topic.ailments[i] = true;
                        }
                        else
                        {
                            throw new System.ArgumentOutOfRangeException($"{topic.Name} does not have {i} Ailments.");
                        }
                    }
                    break;
                case DiscoveryType.Ingredient:
                    foreach(int i in detail.indices)
                    {
                        if(i < Topic.ingredients.Length)
                        {
                            Topic.ingredients[i] = true;
                        }
                        else
                        {
                            throw new System.ArgumentOutOfRangeException($"{topic.Name} does not have {i} Ingredients.");
                        }
                    }
                    break;
                case DiscoveryType.Symptom:
                    foreach(int i in detail.indices)
                    {
                        if(i < Topic.symptoms.Length)
                        {
                            Topic.symptoms[i] = true;
                        }
                        else
                        {
                            throw new System.ArgumentOutOfRangeException($"{topic.Name} does not have {i} Symptom.");
                        }
                    }
                    break;
                case DiscoveryType.Cure:
                    foreach(int i in detail.indices)
                    {
                        if(i < Topic.cures.Length)
                        {
                            Topic.cures[i] = true;
                        }
                        else
                        {
                            throw new System.ArgumentOutOfRangeException($"{topic.Name} does not have {i} Cures.");
                        }
                    }
                    break;
                case DiscoveryType.Location:
                    foreach(int i in detail.indices)
                    {
                        if(i < Topic.locationsFound.Length)
                        {
                            Topic.locationsFound[i] = true;
                        }
                        else
                        {
                            throw new System.ArgumentOutOfRangeException($"{topic.Name} is not found in {i} locations.");
                        }
                    }
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException($"It's impossible to discover that about {topic.Name}.");
            }
        }
    }
}
