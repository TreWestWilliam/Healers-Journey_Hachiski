using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericData : Data
{
    [TextArea(3,5)]
    public string Description;

    [TextArea(3, 5)]
    public string UnlockedDescription;

    public int tier;

    public SpriteRef icon;
}
