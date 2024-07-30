using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TreatedIngredient))]
public class TreatedIngredientDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float extraHeight = (GetPropertyHeight(property, label) / 2) + 1;

        position.height -= extraHeight;

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty ingredient = property.FindPropertyRelative(nameof(TreatedIngredient.ingredient));
        SerializedProperty temperature = property.FindPropertyRelative(nameof(TreatedIngredient.temperature));

        Temperature temp = (Temperature)temperature.enumValueFlag;

        Color initialColor = GUI.color;
        Color tempColor = Color.white;

        switch(temp)
        {
            case Temperature.Cold:
                tempColor = Color.blue;
                break;
            case Temperature.Hot:
                tempColor = Color.red;
                break;
            default:
                tempColor = Color.white;;
                break;
        }

        // Store old indent level and set it to 0, the PrefixLabel takes care of it

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        position.height += extraHeight;

        EditorGUI.PropertyField(position, ingredient, GUIContent.none);

        position.height -= extraHeight;
        position.y += extraHeight;

        float width = position.width;

        position.width = extraHeight * 4;
        position.x -= extraHeight * 5;

        GUI.color = tempColor;

        EditorGUI.PropertyField(position, temperature, GUIContent.none);

        position.height *= 2f;

        position.x += extraHeight * 5;
        position.width = width;

        GUI.color = initialColor;

        if(EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (base.GetPropertyHeight(property, label) * 2) + 2;
    }
}