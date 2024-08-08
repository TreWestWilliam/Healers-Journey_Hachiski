using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using Object = UnityEngine.Object;

[CustomEditor(typeof(IngredientData), true)]
[CanEditMultipleObjects]
public class IngredientDataEditor : GenericDataEditor
{
}
