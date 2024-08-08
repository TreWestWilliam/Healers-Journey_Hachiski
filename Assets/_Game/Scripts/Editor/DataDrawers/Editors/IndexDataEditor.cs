using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(IndexData), true)]
[CanEditMultipleObjects]
public class IndexDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Sort Tier Contents"))
        {
            (target as IndexData).sortIndex();
        }
        if(GUILayout.Button("Reciprocate Indexed Data Relationships"))
        {
            foreach(TierIndex tier in (target as IndexData).tiers)
            {
                if(tier != null)
                {
                    if(tier.data == null)
                    {
                        tier.data = new GenericData[0];
                    }
                    foreach(GenericData data in tier.data)
                    {
                        data.reciprocateData();
                    }
                }
            }
        }
        base.OnInspectorGUI();
    }
}
