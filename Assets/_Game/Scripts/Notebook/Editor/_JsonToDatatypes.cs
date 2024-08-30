using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

class AilmentJsonData
{
    public string NAME;
    public int TIER;
    public string ICON;
    public string DES;
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
        ICON = "";
        DES = "";
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

class IngredientJsonData
{
    public string NAME;
    public int TIER;
    public string ICON;
    public string DES;
    public string SYMP1;
    public string SYMP2;
    public string SYMP3;
    public string SYMP4;
    public string SYMP5;
    public string SYMP6;

    public IngredientJsonData()
    {
        NAME = "";
        TIER = 0;
        ICON = "";
        DES = "";
        SYMP1 = "";
        SYMP2 = "";
        SYMP3 = "";
        SYMP4 = "";
        SYMP5 = "";
        SYMP6 = "";
    }
}

class SymptomJsonData
{
    public string NAME;
    public int TIER;
    public string ICON;
    public string DES;
    public string ING1;
    public string ING2;
    public string ING3;
    public string ING4;
    public string ING5;
    public string COMP;

    public SymptomJsonData()
    {
        NAME = "";
        TIER = 0;
        ICON = "";
        DES = "";
        ING1 = "";
        ING2 = "";
        ING3 = "";
        ING4 = "";
        ING5 = "";
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

    public string iconFolder;
    public string iconList;

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

#if UNITY_EDITOR
    public void createSOs()
    {

        string datafolderPath = "Assets/_Game/Data";

        Dictionary<string, SymptomData> nameToSymptom = new Dictionary<string, SymptomData>();
        Dictionary<string, IngredientData> nameToIngredient = new Dictionary<string, IngredientData>();
        Dictionary<string, AilmentData> nameToAilment = new Dictionary<string, AilmentData>();

        string symptomString = symptomsJson.ToString();
        symptomString = symptomString.Remove(0, 3);
        symptomString = symptomString.Remove(symptomString.Length - 3, 3);

        string[] symptomStrings = symptomString.Split("},");
        for(int i = 0; i <  symptomStrings.Length - 1; i++)
        {
            symptomStrings[i] += "}";
        }

        SymptomData[] symptomSOs = new SymptomData[symptomStrings.Length];
        SymptomJsonData[] symptomStructs = new SymptomJsonData[symptomStrings.Length];

        symptomIndex.tiers = new TierIndex[1];
        symptomIndex.tiers[0] = new TierIndex();
        symptomIndex.tiers[0].data = new GenericData[symptomStrings.Length];

        for(int i = 0; i < symptomStrings.Length; i++)
        {
            symptomStructs[i] = new SymptomJsonData();

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
            if(getFileName(AssetDatabase.GetAssetPath(symptomSOs[i])) != symptomStructs[i].NAME)
            {
                replaceFileName(AssetDatabase.GetAssetPath(symptomSOs[i]), symptomStructs[i].NAME);
            }
            if(AssetDatabase.GetAssetPath(symptomSOs[i]) != filePath)
            {
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(symptomSOs[i]), filePath);
            }

            symptomSOs[i].Name = symptomStructs[i].NAME;
            symptomSOs[i].tier = symptomStructs[i].TIER - 1;

            string iconName = symptomStructs[i].ICON;
            if(iconName == null || iconName == "")
            {
                iconName = symptomStructs[i].NAME;
            }

            string iconPath = searchAllSubfolders(iconFolder, iconName);

            if(iconPath != null)
            {
                string expectedIconPath = iconFolder + "/tier " + symptomStructs[i].TIER + "/" + iconName + ".svg";
                if(iconPath.ToLower() != expectedIconPath.ToLower())
                {
                    AssetDatabase.MoveAsset(iconPath, expectedIconPath);
                }
                symptomSOs[i].icon.UseConstant = true;
                symptomSOs[i].icon.ConstantValue = AssetDatabase.LoadAssetAtPath<Sprite>(expectedIconPath);
            }

            if(symptomStructs[i].DES != null && symptomStructs[i].DES != "")
            {
                symptomSOs[i].Description = symptomStructs[i].DES;
            }

            symptomSOs[i].ingredients = new IngredientData[0];
            symptomSOs[i].ailments = new AilmentData[0];

            if(symptomStructs[i].COMP != null && symptomStructs[i].COMP != "")
            {
                StringRef[] comps = new StringRef[1];
                comps[0].UseConstant = true;
                comps[0].ConstantValue = symptomStructs[i].COMP;
                symptomSOs[i].complaints = comps;
            }

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
        IngredientJsonData[] ingredientStructs = new IngredientJsonData[ingredientStrings.Length];

        ingredientIndex.tiers = new TierIndex[1];
        ingredientIndex.tiers[0] = new TierIndex();
        ingredientIndex.tiers[0].data = new GenericData[ingredientStrings.Length];

        for(int i = 0; i < ingredientStrings.Length; i++)
        {
            ingredientStructs[i] = new IngredientJsonData();

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
            if(getFileName(AssetDatabase.GetAssetPath(ingredientSOs[i])) != ingredientStructs[i].NAME)
            {
                replaceFileName(AssetDatabase.GetAssetPath(ingredientSOs[i]), ingredientStructs[i].NAME);
            }
            if(AssetDatabase.GetAssetPath(ingredientSOs[i]) != filePath)
            {
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(ingredientSOs[i]), filePath);
            }

            ingredientSOs[i].Name = ingredientStructs[i].NAME;
            ingredientSOs[i].tier = ingredientStructs[i].TIER - 1;

            string iconName = ingredientStructs[i].ICON;
            if(iconName == null || iconName == "")
            {
                iconName = ingredientStructs[i].NAME;
            }

            string iconPath = searchAllSubfolders(iconFolder, iconName);

            if(iconPath != null)
            {
                string expectedIconPath = iconFolder + "/tier " + ingredientStructs[i].TIER + "/" + iconName + ".svg";
                if(iconPath.ToLower() != expectedIconPath.ToLower())
                {
                    AssetDatabase.MoveAsset(iconPath, expectedIconPath);
                }
                ingredientSOs[i].icon.UseConstant = true;
                ingredientSOs[i].icon.ConstantValue = AssetDatabase.LoadAssetAtPath<Sprite>(expectedIconPath);
            }

            if(ingredientStructs[i].DES != null && ingredientStructs[i].DES != "")
            {
                ingredientSOs[i].Description = ingredientStructs[i].DES;
            }

            List<SymptomData> ingredientSymptoms = new List<SymptomData>();

            String symptom;

            symptom = ingredientStructs[i].SYMP1.ToLower();
            if(symptom != null && symptom != "" && nameToSymptom.ContainsKey(symptom))
            {
                ingredientSymptoms.Add(nameToSymptom[symptom]);
            }

            symptom = ingredientStructs[i].SYMP2.ToLower();
            if(symptom != null && symptom != "" && nameToSymptom.ContainsKey(symptom))
            {
                ingredientSymptoms.Add(nameToSymptom[symptom]);
            }

            symptom = ingredientStructs[i].SYMP3.ToLower();
            if(symptom != null && symptom != "" && nameToSymptom.ContainsKey(symptom))
            {
                ingredientSymptoms.Add(nameToSymptom[symptom]);
            }

            symptom = ingredientStructs[i].SYMP4.ToLower();
            if(symptom != null && symptom != "" && nameToSymptom.ContainsKey(symptom))
            {
                ingredientSymptoms.Add(nameToSymptom[symptom]);
            }

            symptom = ingredientStructs[i].SYMP5.ToLower();
            if(symptom != null && symptom != "" && nameToSymptom.ContainsKey(symptom))
            {
                ingredientSymptoms.Add(nameToSymptom[symptom]);
            }

            symptom = ingredientStructs[i].SYMP6.ToLower();
            if(symptom != null && symptom != "" && nameToSymptom.ContainsKey(symptom))
            {
                ingredientSymptoms.Add(nameToSymptom[symptom]);
            }

            ingredientSOs[i].symptoms = ingredientSymptoms.ToArray();
            ingredientSOs[i].ailments = new AilmentData[0];

            ingredientSOs[i].reciprocateData();

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
            if(getFileName(AssetDatabase.GetAssetPath(ailmentSOs[i])) != ailmentStructs[i].NAME)
            {
                replaceFileName(AssetDatabase.GetAssetPath(ailmentSOs[i]), ailmentStructs[i].NAME);
            }
            if(AssetDatabase.GetAssetPath(ailmentSOs[i]) != filePath)
            {
                AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(ailmentSOs[i]), filePath);
            }

            ailmentSOs[i].Name = ailmentStructs[i].NAME;
            ailmentSOs[i].tier = ailmentStructs[i].TIER - 1;

            string iconName = ailmentStructs[i].ICON;
            if(iconName == null || iconName == "")
            {
                iconName = ailmentStructs[i].NAME;
            }

            string iconPath = searchAllSubfolders(iconFolder, iconName);

            if(iconPath != null)
            {
                string expectedIconPath = iconFolder + "/tier " + ailmentStructs[i].TIER + "/" + iconName + ".svg";
                if(iconPath.ToLower() != expectedIconPath.ToLower())
                {
                    AssetDatabase.MoveAsset(iconPath, expectedIconPath);
                }
                ailmentSOs[i].icon.UseConstant = true;
                ailmentSOs[i].icon.ConstantValue = AssetDatabase.LoadAssetAtPath<Sprite>(expectedIconPath);
            }

            if(ailmentStructs[i].DES != null && ailmentStructs[i].DES != "")
            {
                ailmentSOs[i].Description = ailmentStructs[i].DES;
            }

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

            if(ailmentStructs[i].COMP != null && ailmentStructs[i].COMP != "")
            {
                ailmentSOs[i].complaints = new StringRef[1];
                ailmentSOs[i].complaints[0] = new StringRef();
                ailmentSOs[i].complaints[0].UseConstant = true;
                ailmentSOs[i].complaints[0].ConstantValue = ailmentStructs[i].COMP;
            }

            ailmentSOs[i].reciprocateData();

            ailmentIndex.tiers[0].data[i] = ailmentSOs[i];
        }
        ailmentIndex.sortIndex();

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private string searchAllSubfolders(string baseFolder, string searchName)
    {
        searchName = searchName.ToLower();
        string[] folders = { baseFolder };
        string[] paths = AssetDatabase.FindAssets(searchName, folders);
        if(paths.Length > 0)
        {
            for(int i = 0; i < paths.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(paths[i]);
                string fileName = getFileName(path).ToLower();
                if(fileName == searchName)
                {
                    return AssetDatabase.GUIDToAssetPath(paths[i]);
                }
            }
            return null;
        }
        return null;
    }

    private string getFileName(string path)
    {
        string fileName = path.Split('/').Last();
        List<string> split = new List<string>(fileName.Split('.'));
        split.RemoveAt(split.Count - 1);
        fileName = String.Join('.', split.ToArray());
        return fileName;
    }

    private void replaceFileName(string path, string newName)
    {
        string[] splitPath = path.Split('/');
        string extension = splitPath.Last().Split('.').Last();
        string fileName = String.Join('.', newName, extension);
        splitPath[splitPath.Length - 1] = fileName;
        AssetDatabase.RenameAsset(path, String.Join('/', splitPath));
    }

    public void listAllIcons()
    {
        string[] folders = { iconFolder };
        string[] paths = AssetDatabase.FindAssets("t:Sprite", folders);
        if(paths.Length > 0)
        {
            string output = "";
            for(int i = 0; i < paths.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(paths[i]);
                string fileName = path.Split('/').Last();
                List<String> split = new List<String>(fileName.Split('.'));
                split.RemoveAt(split.Count - 1);
                fileName = String.Join('.', split.ToArray());
                output += fileName;
                if(i < paths.Length - 1)
                {
                    output += "|";
                }
            }
            iconList = output;
        }
    }
#endif
}

#if UNITY_EDITOR
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
        if(GUILayout.Button("List icons"))
        {
            jsonToDatatypes.listAllIcons();
        }
        base.OnInspectorGUI();
    }
}
#endif
