using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private SVGImage icon;

    [SerializeField] private ItemData itemData;

    [SerializeField] private RecipeIngredientSlot fromSlot;

    private bool disableAfterDrag = false;

    Transform parentAfterDrag;
    private Vector3 mouseOffset;

    public void setItem(ItemData item)
    {
        itemData = item;
        if(itemData == null)
        {
            gameObject.SetActive(false);
            name = "Dragger : Empty";
            icon.sprite = null;
        }
        else
        {
            name = "Dragger : " + itemData.Name;
            icon.sprite = itemData.icon;
            gameObject.SetActive(!disableAfterDrag);
        }
    }

    public void setDisableAfterDrag(bool disable)
    {
        disableAfterDrag = disable;
        if(disable)
        {
            if(icon.raycastTarget)
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            if(itemData != null)
            {
                gameObject.SetActive(true);
            }
        }
    }

    public ItemData getItem()
    {
        return itemData;
    }

    public void clearSlot()
    {
        if(fromSlot != null)
        {
            endDrag();
            fromSlot.clearIngredient();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        mouseOffset = transform.position - Input.mousePosition;
        parentAfterDrag = transform.parent;
        //Debug.Log("Begin Dragging " + name + " with parent " + parentAfterDrag.name);
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        icon.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging " + name);
        transform.position = Input.mousePosition + mouseOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(icon.raycastTarget == false)
        {
            endDrag();
        }
    }

    private void endDrag()
    {
        //Debug.Log("End Dragging " + name);
        if(parentAfterDrag is null)
        {
            Destroy(gameObject);
        }

        //Debug.Log("Parent was " + parentAfterDrag.name);
        transform.SetParent(parentAfterDrag);
        //Debug.Log("Parent is now " + transform.parent.name);
        icon.raycastTarget = true;
        if(disableAfterDrag)
        {
            gameObject.SetActive(false);
        }
    }
}
