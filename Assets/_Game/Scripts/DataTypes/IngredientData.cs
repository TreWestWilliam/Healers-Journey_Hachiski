using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ingredient", menuName = "Data/Ingredient", order = 1)]
public class IngredientData : GenericData
{
    public AilmentData[] ailments;
    public SymptomData[] symptoms;

    public StringRef[] locationsFound;
}
