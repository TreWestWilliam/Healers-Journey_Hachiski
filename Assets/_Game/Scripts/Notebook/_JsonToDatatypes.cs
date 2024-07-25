using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

class NameAndTier
{
    public string NAME;
    public int TIER;

    public NameAndTier(string name, int tier)
    {
        NAME = name;
        TIER = tier;
    }
}

class AilmentJsonData
{
    public string NAME;
    public int TIER;
    public string ING1;
    public string TEMP1;
    public string ING2;
    public string TEMP2;
    public string ING3;
    public string TEMP3;
    public string SYMP1;
    public string SYMP2;
    public string COMP;

    public AilmentJsonData()
    {
        NAME = "";
        TIER = 0;
        ING1 = "";
        TEMP1 = "";
        ING2 = "";
        TEMP2 = "";
        ING3 = "";
        TEMP3 = "";
        SYMP1 = "";
        SYMP2 = "";
        COMP = "";
    }
}

//[CreateAssetMenu(fileName = "!jsonToData", menuName = "Data/jsonToData", order = 20)]
public class _JsonToDatatypes : ScriptableObject
{
    public TextAsset ailmentsJson;
    public TextAsset ingredientsJson;
    public TextAsset symptomsJson;

    public AilmentIndex ailmentIndex;
    public IngredientIndex ingredientIndex;
    public SymptomIndex symptomIndex;

    private Temperature stringToTemp(string text)
    {
        switch(text)
        {
            case "Cold":
                return Temperature.Cold;
            case "Hot":
                return Temperature.Hot;
            default:
                return Temperature.Default;
        }
    }

    public void createSOs()
    {

        string datafolderPath = "Assets/_Game/Data";

        Dictionary<string, SymptomData> nameToSymptom = new Dictionary<string, SymptomData>();
        Dictionary<string, IngredientData> nameToIngredient = new Dictionary<string, IngredientData>();
        Dictionary<string, AilmentData> nameToAilment = new Dictionary<string, AilmentData>();

        /*if(symptomIndex.tiers != null)
        {
            foreach(TierIndex tier in symptomIndex.tiers)
            {
                if(tier.data != null)
                {
                    foreach(GenericData data in tier.data)
                    {
                        if(data != null && !nameToSymptom.ContainsKey(data.Name.ToLower()))
                        {
                            string fileName = AssetDatabase.GetAssetPath(data).Split('/').Last().ToLower();
                            nameToSymptom.Add(fileName.Remove(fileName.Length - 6), data as SymptomData);
                        }
                    }
                }
            }
        }

        if(ingredientIndex.tiers != null)
        {
            foreach(TierIndex tier in ingredientIndex.tiers)
            {
                if(tier.data != null)
                {
                    foreach(GenericData data in tier.data)
                    {
                        if(data != null && !nameToIngredient.ContainsKey(data.Name.ToLower()))
                        {
                            string fileName = AssetDatabase.GetAssetPath(data).Split('/').Last().ToLower();
                            nameToIngredient.Add(fileName.Remove(fileName.Length - 6), data as IngredientData);
                        }
                    }
                }
            }
        }

        if(ailmentIndex.tiers != null)
        {
            foreach(TierIndex tier in ailmentIndex.tiers)
            {
                if(tier.data != null)
                {
                    foreach(GenericData data in tier.data)
                    {
                        if(data != null && !nameToAilment.ContainsKey(data.Name.ToLower()))
                        {
                            string fileName = AssetDatabase.GetAssetPath(data).Split('/').Last().ToLower();
                            nameToAilment.Add(fileName.Remove(fileName.Length - 6), data as AilmentData);
                        }
                    }
                }
            }
        }*/

        string symptomString = symptomsJson.ToString();
        symptomString = symptomString.Remove(0, 3);
        symptomString = symptomString.Remove(symptomString.Length - 3, 3);

        string[] symptomStrings = symptomString.Split("},");
        for(int i = 0; i <  symptomStrings.Length - 1; i++)
        {
            symptomStrings[i] += "}";
        }

        SymptomData[] symptomSOs = new SymptomData[symptomStrings.Length];
        NameAndTier[] symptomStructs = new NameAndTier[symptomStrings.Length];

        symptomIndex.tiers = new TierIndex[1];
        symptomIndex.tiers[0] = new TierIndex();
        symptomIndex.tiers[0].data = new GenericData[symptomStrings.Length];

        for(int i = 0; i < symptomStrings.Length; i++)
        {
            symptomStructs[i] = new NameAndTier("", 0);

            EditorJsonUtility.FromJsonOverwrite(symptomStrings[i], symptomStructs[i]);

            string filePath = datafolderPath + "/Symptoms/Tier " + symptomStructs[i].TIER + "/" + symptomStructs[i].NAME + ".asset";


            if(!nameToSymptom.ContainsKey(symptomStructs[i].NAME.ToLower()))
            {
                string existingPath = searchAllSubfolders(datafolderPath + "/Symptoms", symptomStructs[i].NAME);
                if(existingPath != null)
                {
                    symptomSOs[i] = AssetDatabase.LoadAssetAtPath<SymptomData>(existingPath);
                }
                else
                {
                    symptomSOs[i] = ScriptableObject.CreateInstance<SymptomData>();
                    AssetDatabase.CreateAsset(symptomSOs[i], filePath);
                }
                symptomSOs[i].name = symptomStructs[i].NAME;
                nameToSymptom.Add(symptomStructs[i].NAME.ToLower(), symptomSOs[i]);
            }
            else
            {
                symptomSOs[i] = nameToSymptom[symptomStructs[i].NAME.ToLower()];
            }
            if(AssetDatabase.GetAssetPath(symptomSOs[i]) != filePath)
            {
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(symptomSOs[i]), filePath);
            }

            symptomSOs[i].Name = symptomStructs[i].NAME;
            symptomSOs[i].tier = symptomStructs[i].TIER - 1;

            //Debug.Log(symptomSOs[i].Name + " : Tier " + symptomSOs[i].tier);

            symptomIndex.tiers[0].data[i] = symptomSOs[i];
        }
        symptomIndex.sortIndex();

        string ingredientsString = ingredientsJson.ToString();
        ingredientsString = ingredientsString.Remove(0, 3);
        ingredientsString = ingredientsString.Remove(ingredientsString.Length - 3, 3);

        string[] ingredientStrings = ingredientsString.Split("},");
        for(int i = 0; i < ingredientStrings.Length - 1; i++)
        {
            ingredientStrings[i] += "}";
        }

        IngredientData[] ingredientSOs = new IngredientData[ingredientStrings.Length];
        NameAndTier[] ingredientStructs = new NameAndTier[ingredientStrings.Length];

        ingredientIndex.tiers = new TierIndex[1];
        ingredientIndex.tiers[0] = new TierIndex();
        ingredientIndex.tiers[0].data = new GenericData[ingredientStrings.Length];

        for(int i = 0; i < ingredientStrings.Length; i++)
        {
            ingredientStructs[i] = new NameAndTier("", 0);

            EditorJsonUtility.FromJsonOverwrite(ingredientStrings[i], ingredientStructs[i]);

            string filePath = datafolderPath + "/Ingredients/Tier " + ingredientStructs[i].TIER + "/" + ingredientStructs[i].NAME + ".asset";


            if(!nameToIngredient.ContainsKey(ingredientStructs[i].NAME.ToLower()))
            {
                string existingPath = searchAllSubfolders(datafolderPath + "/Ingredients", ingredientStructs[i].NAME);
                if(existingPath != null)
                {
                    ingredientSOs[i] = AssetDatabase.LoadAssetAtPath<IngredientData>(existingPath);
                }
                else
                {
                    ingredientSOs[i] = ScriptableObject.CreateInstance<IngredientData>();
                    AssetDatabase.CreateAsset(ingredientSOs[i], filePath);
                }
                ingredientSOs[i].name = ingredientStructs[i].NAME;
                nameToIngredient.Add(ingredientStructs[i].NAME.ToLower(), ingredientSOs[i]);
            }
            else
            {
                ingredientSOs[i] = nameToIngredient[ingredientStructs[i].NAME.ToLower()];
            }
            if(AssetDatabase.GetAssetPath(ingredientSOs[i]) != filePath)
            {
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(ingredientSOs[i]), filePath);
            }

            ingredientSOs[i].Name = ingredientStructs[i].NAME;
            ingredientSOs[i].tier = ingredientStructs[i].TIER - 1;

            ingredientIndex.tiers[0].data[i] = ingredientSOs[i];
        }
        ingredientIndex.sortIndex();


        string ailmentsString = ailmentsJson.ToString();
        ailmentsString = ailmentsString.Remove(0, 3);
        ailmentsString = ailmentsString.Remove(ailmentsString.Length - 3, 3);

        string[] ailmentStrings = ailmentsString.Split("},");
        for(int i = 0; i < ailmentStrings.Length - 1; i++)
        {
            ailmentStrings[i] += "}";
        }

        AilmentData[] ailmentSOs = new AilmentData[ailmentStrings.Length];
        AilmentJsonData[] ailmentStructs = new AilmentJsonData[ailmentStrings.Length];

        ailmentIndex.tiers = new TierIndex[1];
        ailmentIndex.tiers[0] = new TierIndex();
        ailmentIndex.tiers[0].data = new GenericData[ailmentStrings.Length];

        for(int i = 0; i < ailmentStrings.Length; i++)
        {
            ailmentStructs[i] = new AilmentJsonData();

            EditorJsonUtility.FromJsonOverwrite(ailmentStrings[i], ailmentStructs[i]);

            string filePath = datafolderPath + "/Ailments/Tier " + ailmentStructs[i].TIER + "/" + ailmentStructs[i].NAME + ".asset";

            if(!nameToAilment.ContainsKey(ailmentStructs[i].NAME.ToLower()))
            {
                string existingPath = searchAllSubfolders(datafolderPath + "/Ailments", ailmentStructs[i].NAME);
                if(existingPath != null)
                {
                    ailmentSOs[i] = AssetDatabase.LoadAssetAtPath<AilmentData>(existingPath);
                }
                else
                {
                    ailmentSOs[i] = ScriptableObject.CreateInstance<AilmentData>();
                    AssetDatabase.CreateAsset(ailmentSOs[i], filePath);
                }
                ailmentSOs[i].name = ailmentStructs[i].NAME;
                nameToAilment.Add(ailmentStructs[i].NAME.ToLower(), ailmentSOs[i]);
            }
            else
            {
                ailmentSOs[i] = nameToAilment[ailmentStructs[i].NAME.ToLower()];
            }
            if(AssetDatabase.GetAssetPath(ailmentSOs[i]) != filePath)
            {
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(ailmentSOs[i]), filePath);
            }

            ailmentSOs[i].Name = ailmentStructs[i].NAME;
            ailmentSOs[i].tier = ailmentStructs[i].TIER - 1;

            ailmentSOs[i].cures = new Cure[1];
            ailmentSOs[i].cures[0] = new Cure();

            TreatedIngredient tI1 = new TreatedIngredient(nameToIngredient[ailmentStructs[i].ING1.ToLower()], stringToTemp(ailmentStructs[i].TEMP1));
            TreatedIngredient tI2 = new TreatedIngredient(nameToIngredient[ailmentStructs[i].ING2.ToLower()], stringToTemp(ailmentStructs[i].TEMP2));
            TreatedIngredient tI3 = new TreatedIngredient(nameToIngredient[ailmentStructs[i].ING3.ToLower()], stringToTemp(ailmentStructs[i].TEMP3));

            TreatedIngredient[] recipe = { tI1, tI2, tI3 };

            ailmentSOs[i].cures[0].recipe = recipe;

            ailmentSOs[i].symptoms = new SymptomData[2];
            ailmentSOs[i].symptoms[0] = nameToSymptom[ailmentStructs[i].SYMP1.ToLower()];
            ailmentSOs[i].symptoms[1] = nameToSymptom[ailmentStructs[i].SYMP2.ToLower()];

            ailmentSOs[i].complaints = new StringRef[1];
            ailmentSOs[i].complaints[0] = new StringRef();
            ailmentSOs[i].complaints[0].UseConstant = true;
            ailmentSOs[i].complaints[0].ConstantValue = ailmentStructs[i].COMP;

            ailmentSOs[i].reciprocateData();

            ailmentIndex.tiers[0].data[i] = ailmentSOs[i];
        }
        ailmentIndex.sortIndex();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private string searchAllSubfolders(string baseFolder, string name)
    {
        string[] folders = { baseFolder };
        string[] paths = AssetDatabase.FindAssets(name, folders);
        if(paths.Length > 0)
        {
            return AssetDatabase.GUIDToAssetPath(paths[0]);
        }
        return null;
    }
}



[CustomEditor(typeof(_JsonToDatatypes), true)]
[CanEditMultipleObjects]
public class _JsonToDatatypesEditor : Editor
{
    private _JsonToDatatypes jsonToDatatypes { get { return (target as _JsonToDatatypes); } }
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Create ScriptableObjects"))
        {
            jsonToDatatypes.createSOs();
        }
        base.OnInspectorGUI();
    }
}