using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class TitlePage : MonoBehaviour
{
    public TitleData titleData;
    public TMP_Text dedication;

    private void Start()
    {
        foreach(string credit in titleData.credits)
        {
            string divider = credit.Last() == '>' ? " " : ", ";
            dedication.text += credit + divider;
        }
        if(titleData.credits.Length > 0)
        {
            dedication.text = dedication.text.Remove(dedication.text.Length - 3, 2) + ".</size>";
        }
    }
}
