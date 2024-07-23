using System;

[Serializable]
public class BoolRef : IEquatable<BoolRef>, IComparable<BoolRef>
{
    public bool UseConstant = true;
    public bool ConstantValue;
    public BoolVariable Variable;

    public bool Value
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

    public static implicit operator bool(BoolRef v)
    {
        return v.Value;
    }

    public static bool operator ==(BoolRef v1, BoolRef v2) 
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

    public static bool operator !=(BoolRef v1, BoolRef v2) => !(v1 == v2);

    public bool Equals(BoolRef other)
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
        return (obj is BoolRef @ref && Value.Equals(@ref.Value)) || (obj is bool @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }

    public int CompareTo(BoolRef other)
    {
        return Value.CompareTo(other.Value);
    }
}