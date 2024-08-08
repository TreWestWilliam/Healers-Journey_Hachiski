using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NotebookHandler), true)]
[CanEditMultipleObjects]
public class NotebookHandlerEditor : Editor
{
    private NotebookHandler notebookHandler { get { return (target as NotebookHandler); } }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Discover Tier 1 Ingredients & Symptoms"))
        {
            notebookHandler.DiscoverTier1IngredientsAndSymptoms();
        }
        if(GUILayout.Button("Discover Tier 1 Ailments"))
        {
            notebookHandler.DiscoverTier1Ailments();
        }
        if(GUILayout.Button("Discover Tier 1 Ailment Symptoms"))
        {
            notebookHandler.DiscoverTier1AilmentSymptoms();
        }
        base.OnInspectorGUI();
    }
}
