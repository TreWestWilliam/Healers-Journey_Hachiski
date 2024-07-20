using System;
using UnityEngine;

[Serializable]
public class ColorRef
{
    public bool UseConstant = true;
    public Color ConstantValue = Color.white;
    public ColorVariable Variable;

    public Color Value
    {
        get { return UseConstant ? ConstantValue : (Variable != null) ? Variable.Value : default; }
        set
        {
            if(UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }
    }

    public static implicit operator Color(ColorRef v)
    {
        return v.Value;
    }
}