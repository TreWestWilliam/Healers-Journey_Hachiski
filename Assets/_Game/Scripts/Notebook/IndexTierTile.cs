using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IndexTierTile : MonoBehaviour
{
    public TMP_Text tierLable;
    public GridLayoutGroup entryGrid;

    public IEnumerator updateGrid(int count)
    {
        yield return new WaitForFixedUpdate();

        RectTransform tierTileTransform = entryGrid.transform as RectTransform;
        GridLayoutGroup eG = entryGrid;

        int columns = Mathf.FloorToInt((tierTileTransform.rect.width - eG.padding.left - eG.padding.right + eG.spacing.x) / (eG.cellSize.x + eG.spacing.x));

        int rows = Mathf.CeilToInt((float)(count) / (float)columns);

        float height = ((eG.cellSize.y + eG.spacing.y) * (float)rows) + eG.padding.top + eG.padding.bottom - eG.spacing.y;
        float width = tierTileTransform.sizeDelta.x;
        if(height > tierTileTransform.rect.height)
        {
            tierTileTransform.sizeDelta = new Vector2(width, height - tierTileTransform.rect.height);
        }
    }
}
