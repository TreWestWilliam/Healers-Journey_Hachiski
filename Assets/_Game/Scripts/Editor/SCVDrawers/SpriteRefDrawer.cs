using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpriteRef))]
public class SpriteRefDrawer : PropertyDrawer
{
    /// Options to display in the popup to select constant or variable.
    private readonly string[] popupOptions =
        { "Use Constant", "Use Variable" };

    /// Cached style to use to draw the popup button.
    private GUIStyle popupStyle;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        popupStyle ??= new GUIStyle(GUI.skin.GetStyle("PaneOptions"))
            {
                imagePosition = ImagePosition.ImageOnly
            };

        float extraHeight = (GetPropertyHeight(property, label) / 2) + 1;

        position.height -= extraHeight;

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get properties
        SerializedProperty useConstant = property.FindPropertyRelative("UseConstant");
        SerializedProperty constantValue = property.FindPropertyRelative("ConstantValue");
        SerializedProperty variable = property.FindPropertyRelative("Variable");

        // Calculate rect for configuration button
        Rect buttonRect = new Rect(position);
        buttonRect.yMin += popupStyle.margin.top;
        buttonRect.width = popupStyle.fixedWidth + popupStyle.margin.right;
        position.xMin = buttonRect.xMax + 2;

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        int result = EditorGUI.Popup(buttonRect, useConstant.boolValue ? 0 : 1, popupOptions, popupStyle);

        useConstant.boolValue = result == 0;

        Sprite value = useConstant.boolValue ? (constantValue.objectReferenceValue as Sprite) : (variable.objectReferenceValue ? (variable.objectReferenceValue as SpriteVariable).Value : null);

        Rect spriteRect;

        // Create object field for the sprite.
        spriteRect = new Rect(position.x, position.y, position.height + extraHeight, position.height + extraHeight);
        position.xMin += position.height + extraHeight + 2;
                
        // Skip this if not a repaint or the property is null.
        if(Event.current.type == EventType.Repaint && value != null)
            DrawTexturePreview(spriteRect, value);

        if(useConstant.boolValue)
            EditorGUI.LabelField(position, "Constant:");

        position.y += extraHeight;

        if(!useConstant.boolValue && variable.objectReferenceValue)
        {
            GUI.enabled = false;
            EditorGUI.ObjectField(position, "", value, typeof(Sprite), false);
            GUI.enabled = true;
        }

        if(!useConstant.boolValue)
            position.y -= extraHeight;

        EditorGUI.PropertyField(position,
            useConstant.boolValue ? constantValue : variable,
            GUIContent.none);

        if(EditorGUI.EndChangeCheck())
            property.serializedObject.ApplyModifiedProperties();

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return (base.GetPropertyHeight(property, label) * 2) + 2;
    }

    private void DrawTexturePreview(Rect position, Sprite sprite)
    {
        Vector2 fullSize = new Vector2(sprite.texture.width, sprite.texture.height);
        Vector2 size = new Vector2(sprite.textureRect.width, sprite.textureRect.height);

        Rect coords = sprite.textureRect;
        coords.x /= fullSize.x;
        coords.width /= fullSize.x;
        coords.y /= fullSize.y;
        coords.height /= fullSize.y;

        Vector2 ratio;
        ratio.x = position.width / size.x;
        ratio.y = position.height / size.y;
        float minRatio = Mathf.Min(ratio.x, ratio.y);

        Vector2 center = position.center;
        position.width = size.x * minRatio;
        position.height = size.y * minRatio;
        position.center = center;

        GUI.DrawTextureWithTexCoords(position, sprite.texture, coords);
    }
}