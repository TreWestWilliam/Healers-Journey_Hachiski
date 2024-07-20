using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Temperature
{
    Cold = -1,
    Default = 0,
    Hot = 1
}

[Serializable]
public class TreatedIngredient
{
    public IngredientData ingredient;
    public Temperature temperature;
}


[Serializable]
public class Cure
{
    public StringRef cureName;

    public SpriteRef cureSprite;

    public TreatedIngredient[] recipe;
}

[CreateAssetMenu(fileName = "Ailment", menuName = "Data/Ailment", order = 0)]
public class AilmentData : GenericData
{
    public SymptomData[] symptoms;

    public Cure[] cures;
}
