using System;
using UnityEngine;

[Serializable]
public class ColorRef : IEquatable<ColorRef>
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

    public static bool operator ==(ColorRef v1, ColorRef v2)
    {
        if(v1 is null)
        {
            if(v2 is null)
            {
                return true;
            }
            return false;
        }
        return v1.Value == v2.Value;
    }

    public static bool operator !=(ColorRef v1, ColorRef v2) => !(v1 == v2);

    public bool Equals(ColorRef other)
    {
        if(this is null)
        {
            if(other is null)
            {
                return true;
            }
            return false;
        }

        return Value.Equals(other.Value);
    }

    public override bool Equals(object obj)
    {
        return (obj is ColorRef @ref && Value.Equals(@ref.Value)) || (obj is Color @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}