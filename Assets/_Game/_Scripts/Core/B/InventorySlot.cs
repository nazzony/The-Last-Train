using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount != 0) return;

        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if (draggableItem == null) return;
        draggableItem.parentAfterDrag = transform;
    }
}