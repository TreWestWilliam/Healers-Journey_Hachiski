using System;
using UnityEngine;

[Serializable]
public class SpriteRef
{
    public bool UseConstant = true;
    public Sprite ConstantValue;
    public SpriteVariable Variable;

    public Sprite Value
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

    public static implicit operator Sprite(SpriteRef v)
    {
        return v.Value;
    }
}