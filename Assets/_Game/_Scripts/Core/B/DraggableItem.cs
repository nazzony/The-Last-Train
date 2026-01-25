using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;

    [Header("Item Data")]
    public ItemData itemData;
    public InventoryManager inventory;

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        if (image) image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool accepted = false;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

            if (hit.collider != null)
            {
                var receiver = hit.collider.GetComponent<IItemReceiver>()
                               ?? hit.collider.GetComponentInParent<IItemReceiver>();

                if (receiver != null)
                    accepted = receiver.TryAcceptItem(itemData, inventory);
            }
        }

        if (accepted)
        {
            Destroy(gameObject);
            return;
        }

        transform.SetParent(parentAfterDrag);
        if (image) image.raycastTarget = true;
    }
}