using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;

    [Header("Item Data")]
    public ItemData itemData;
    public InventoryManager inventory; // призначити при створенні UI-іконки

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
        // 1) Якщо кинули в UI слот - InventorySlot виставить parentAfterDrag
        // 2) Якщо НЕ в слот - пробуємо дропнути у світ

        bool droppedOnSlot = parentAfterDrag != null && parentAfterDrag != transform.root;

        // Перевіряємо, чи pointer зараз над UI елементом слота
        // (простий спосіб: якщо новий parentAfterDrag - не старий root)
        // Але ще треба спробувати world drop, якщо слот не змінився.

        // World drop, якщо ми не перемістились у новий слот:
        bool shouldTryWorldDrop = parentAfterDrag == null || parentAfterDrag == transform.root;

        // Частіший кейс: parentAfterDrag залишився старим (ми повернемось), але ми хочемо спробувати world drop
        shouldTryWorldDrop = true;

        bool acceptedByWorld = false;

        if (shouldTryWorldDrop)
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 rayPos = new Vector2(worldPos.x, worldPos.y);

            RaycastHit2D hit = Physics2D.Raycast(rayPos, Vector2.zero);

            if (hit.collider != null)
            {
                // шукаємо IItemReceiver на цьому об’єкті або в батьках
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
