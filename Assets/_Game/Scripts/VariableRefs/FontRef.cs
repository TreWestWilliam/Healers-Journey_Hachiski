using System;
using TMPro;

[Serializable]
public class FontRef : IEquatable<FontRef>
{
    public bool UseConstant = true;
    public TMP_FontAsset ConstantValue;
    public FontVariable Variable;

    public TMP_FontAsset Value
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

    public static implicit operator TMP_FontAsset(FontRef v)
    {
        return v.Value;
    }

    public static bool operator ==(FontRef v1, FontRef v2)
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

    public static bool operator !=(FontRef v1, FontRef v2) => !(v1 == v2);

    public bool Equals(FontRef other)
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
        return (obj is FontRef @ref && Value.Equals(@ref.Value)) || (obj is TMP_FontAsset @c && Value.Equals(@c));
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value);
    }
}