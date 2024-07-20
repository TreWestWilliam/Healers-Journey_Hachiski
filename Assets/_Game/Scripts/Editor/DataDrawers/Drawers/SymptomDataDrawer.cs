using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomPropertyDrawer(typeof(SymptomData))]
public class SymptomDataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        float extraHeight = (GetPropertyHeight(property, label) / 2) + 1;

        position.height -= extraHeight;

        label = EditorGUI.BeginProperty(position, label, property);
        position = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginChangeCheck();

        // Get values

        Sprite icon = null;
        string Name = null;

        if(property.objectReferenceValue)
        {
            icon = (property.objectReferenceValue as SymptomData).icon.Value;
            Name = (property.objectReferenceValue as SymptomData).Name;
        }

        // Store old indent level and set it to 0, the PrefixLabel takes care of it
        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        Rect spriteRect;

        // Create object field for the sprite.
        spriteRect = new Rect(position.x, position.y, position.height + extraHeight, position.height + extraHeight);
        position.xMin += position.height + extraHeight + 2;

        // Skip this if not a repaint or the property is null.
        if(Event.current.type == EventType.Repaint && icon != null)
            DrawTexturePreview(spriteRect, icon);

        EditorGUI.PropertyField(position, property, GUIContent.none);

        position.y += extraHeight;

        EditorGUI.LabelField(position, Name);

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
