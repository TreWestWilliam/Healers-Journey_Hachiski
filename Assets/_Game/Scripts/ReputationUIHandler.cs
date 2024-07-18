using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReputationUIHandler : MonoBehaviour
{
    public Reputation rep;

    [Header("Sliders")]
    public Slider[] sliders;
    public ColorRef[] SliderEmptyColor;
    public ColorRef[] SliderFilledColor;
    public ColorRef[] SliderHandleColor;
    public SpriteRef[] SliderEmptySprite;
    public SpriteRef[] SliderFilledSprite;
    public SpriteRef[] SliderHandleSprite;

    [Header("Tier Text")]
    public TMP_Text[] tierTexts;
    public FontRef[] tierFonts;
    public ColorRef[] tierColors;
    public StringRef[] tierStrings;

    [Header("Progress Text")]
    public TMP_Text[] progressTexts;
    public FontRef[] progressFonts;
    public ColorRef[] progressColors;
    public FontStyles[] progressStyles;

    // Start is called before the first frame update
    void Start()
    {
        findRepTracker();
        setReputation();
    }

    private void findRepTracker()
    {
        if(rep != null)
        {
            return;
        }

        // Find and asign reputation.  Requires more progress on scene setup.

        if(rep != null)
        {
            rep.ui = this;
        }
    }

    public void setReputation()
    {
        setSliders();
    }

    private void setSliders()
    {
        foreach(Slider slider in sliders)
        {
            slider.value = rep.ratioToNextTier;

            Image sliderPart = slider.transform.Find("Background").GetComponent<Image>();
            sliderPart.color = getElementByRep<ColorRef>(SliderEmptyColor);
            sliderPart.sprite = getElementByRep<SpriteRef>(SliderEmptySprite);

            sliderPart = slider.fillRect.GetComponent<Image>();
            sliderPart.color = getElementByRep<ColorRef>(SliderFilledColor);
            sliderPart.sprite = getElementByRep<SpriteRef>(SliderFilledSprite);

            sliderPart = slider.handleRect.GetComponent<Image>();
            sliderPart.color = getElementByRep<ColorRef>(SliderHandleColor);
            sliderPart.sprite = getElementByRep<SpriteRef>(SliderHandleSprite);
        }

        foreach(TMP_Text tierText in tierTexts)
        {
            tierText.font = getElementByRep<FontRef>(tierFonts);
            tierText.color = getElementByRep<ColorRef>(tierColors);
            tierText.text = getElementByRep<StringRef>(tierStrings);
        }

        foreach(TMP_Text progressText in progressTexts)
        {
            if(rep.atMaxTier)
            {
                progressText.text = "Max + " + rep.CurrentTierRep;
            }
            else
            {
                progressText.text = rep.CurrentTierRep + "/" + rep.ReputationPerTier[rep.RepTier];
            }
            progressText.font = getElementByRep<FontRef>(progressFonts);
            progressText.color = getElementByRep<ColorRef>(progressColors);
            progressText.fontStyle = getElementByRep<FontStyles>(progressStyles);
        }
    }

    private T getElementByRep<T>(T[] array)
    {
        int tier = rep.RepTier;
        if(tier >= array.Length)
        {
            tier = array.Length - 1;
        }
        return array[tier];
    }
}
