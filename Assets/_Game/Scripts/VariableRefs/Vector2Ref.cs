using System;
using UnityEngine;

[Serializable]
public class Vector2Ref
{
    public bool UseConstant = true;
    public Vector2 ConstantValue;
    public Vector2Variable Variable;

    public Vector2 Value
    {
        get { return UseConstant ? ConstantValue : Variable.Value; }
        set
        {
            if(UseConstant)
                ConstantValue = value;
            else
                Variable.Value = value;
        }
    }

    public static implicit operator Vector2(Vector2Ref v)
    {
        return v.Value;
    }
}