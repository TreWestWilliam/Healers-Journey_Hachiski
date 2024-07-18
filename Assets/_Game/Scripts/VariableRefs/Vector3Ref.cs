using System;
using UnityEngine;

[Serializable]
public class Vector3Ref
{
    public bool UseConstant = true;
    public Vector3 ConstantValue;
    public Vector3Variable Variable;

    public Vector3 Value
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

    public static implicit operator Vector3(Vector3Ref v)
    {
        return v.Value;
    }
}