using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Symptom", menuName = "Data/Symptom", order = 2)]
public class SymptomData : GenericData
{
    public AilmentData[] ailments;
    public IngredientData[] ingredients;

    public StringRef[] complaints;

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
}
