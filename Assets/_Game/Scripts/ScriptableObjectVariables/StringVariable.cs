using UnityEngine;

[CreateAssetMenu(fileName = "String", menuName = "Variables/String", order = 6)]
public class StringVariable : ScriptableObject
{
    [TextArea(1,7)]
    public string Value;
}