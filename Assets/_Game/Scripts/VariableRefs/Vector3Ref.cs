using System;
using UnityEngine;

[Serializable]
public class Vector3Ref : IEquatable<Vector3Ref>
{
    public bool UseConstant = true;
    public Vector3 ConstantValue;
    public Vector3Variable Variable;

    public Vector3 Value
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

    public static implicit operator Vector3(Vector3Ref v)
    {
        return v.Value;
    }

    public static bool operator ==(Vector3Ref v1, Vector3Ref v2)
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

    public static bool operator !=(Vector3Ref v1, Vector3Ref v2) => !(v1 == v2);

    public bool Equals(Vector3Ref other)
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
        return (obj is Vector3Ref @ref && Value.Equals(@ref.Value)) || (obj is Vector3 @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}