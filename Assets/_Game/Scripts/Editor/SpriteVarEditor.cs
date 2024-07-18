using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomEditor(typeof(SpriteVariable), true)]
[CanEditMultipleObjects]
public class SpriteVarEditor : Editor
{
    private SpriteVariable spriteVar { get { return (target as SpriteVariable); } }

    public override Texture2D RenderStaticPreview(string assetPath, Object[] subAssets, int width, int height)
    {
        if(spriteVar.Value != null)
        {
            Type t = GetType("UnityEditor.SpriteUtility");
            if(t != null)
            {
                MethodInfo method = t.GetMethod("RenderStaticPreview", new Type[] { typeof(Sprite), typeof(Color), typeof(int), typeof(int) });
                if(method != null)
                {
                    object ret = method.Invoke("RenderStaticPreview", new object[] { spriteVar.Value, Color.white, width, height });
                    if(ret is Texture2D)
                        return ret as Texture2D;
                }
            }

            //return ((SpriteVariable)serializedObject.targetObject).Value.texture;
        }
        return base.RenderStaticPreview(assetPath, subAssets, width, height);
    }

    public override void OnInspectorGUI()
    {
        GUI.Label(new Rect(20, 5, 35, EditorGUIUtility.singleLineHeight), "Value");

        if(spriteVar.Value != null)
            DrawTexturePreview(new Rect(67, 5, 60, 60), spriteVar.Value);

        Rect rect = new Rect(Screen.width / 2f, 5, (Screen.width / 2f), EditorGUIUtility.singleLineHeight);

        EditorGUI.indentLevel = 8;
        spriteVar.Value = (Sprite)EditorGUILayout.ObjectField(spriteVar.Value, typeof(Sprite), false, GUILayout.Width((Screen.width / 1f) - 23f), GUILayout.Height(EditorGUIUtility.singleLineHeight));
        EditorUtility.SetDirty(spriteVar);
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

    private static Type GetType(string TypeName)
    {
        var type = Type.GetType(TypeName);
        if(type != null)
            return type;

        if(TypeName.Contains("."))
        {
            var assemblyName = TypeName.Substring(0, TypeName.IndexOf('.'));
            var assembly = Assembly.Load(assemblyName);
            if(assembly == null)
                return null;
            type = assembly.GetType(TypeName);
            if(type != null)
                return type;
        }

        var currentAssembly = Assembly.GetExecutingAssembly();
        var referencedAssemblies = currentAssembly.GetReferencedAssemblies();
        foreach(var assemblyName in referencedAssemblies)
        {
            var assembly = Assembly.Load(assemblyName);
            if(assembly != null)
            {
                type = assembly.GetType(TypeName);
                if(type != null)
                    return type;
            }
        }
        return null;
    }
}