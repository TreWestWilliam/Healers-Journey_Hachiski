using System;
using TMPro;

[Serializable]
public class FontRef
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
}