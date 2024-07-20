using System;

[Serializable]
public class BoolRef
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
}