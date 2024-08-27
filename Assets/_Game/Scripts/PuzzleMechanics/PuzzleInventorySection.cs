using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
//using static Unity.VisualScripting.ReorderableList.ReorderableListControl;

public class PuzzleInventorySection : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        ItemDragger dragger = dropped.GetComponent<ItemDragger>();
        if(dragger != null)
        {
            dragger.clearSlot();
        }
    }
}
