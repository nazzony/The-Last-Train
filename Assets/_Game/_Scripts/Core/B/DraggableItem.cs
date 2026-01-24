using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;

    [Header("Item Data")]
    public ItemData itemData;
    public InventoryManager inventory; 

    private Canvas rootCanvas;

    private void Awake()
    {
        rootCanvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        if (image != null) image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        bool droppedOnSlot = parentAfterDrag != null && parentAfterDrag != transform.root;
        
        bool shouldTryWorldDrop = parentAfterDrag == null || parentAfterDrag == transform.root;
        
        shouldTryWorldDrop = true;

        bool acceptedByWorld = false;

        if (shouldTryWorldDrop)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rayPos = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider != null)
            {
                var receiver = hit.collider.GetComponent<IItemReceiver>();
                if (receiver == null)
                    receiver = hit.collider.GetComponentInParent<IItemReceiver>();

                if (receiver != null && itemData != null && inventory != null)
                {
                    acceptedByWorld = receiver.TryAcceptItem(itemData, inventory);
                }
            }
        }

        if (acceptedByWorld)
        {
            Destroy(gameObject);
            return;
        }
        
        if (parentAfterDrag == null) parentAfterDrag = transform.root;

        transform.SetParent(parentAfterDrag);
        if (image != null) image.raycastTarget = true;
    }
}
