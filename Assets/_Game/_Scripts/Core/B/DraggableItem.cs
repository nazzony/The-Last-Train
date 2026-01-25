using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * UI-іконка предмета, яку можна перетягувати.
 *
 * Функціонал:
 * - drag в межах UI (перетягування між слотами)
 * - drop "в світ" (якщо відпустили не над UI):
 *   робиться Physics2D перевірка в точці курсору,
 *   шукається IItemReceiver і передається предмет.
 
 * Якщо receiver прийняв предмет (повернув true) — іконка знищується.
 * ВАЖЛИВО: receiver сам вирішує, чи витрачати предмет в інвентарі.
 */

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
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
        
        if (rootCanvas != null)
            transform.SetParent(rootCanvas.transform);
        else
            transform.SetParent(transform.root); // fallback, якщо Canvas не знайшли

        transform.SetAsLastSibling();

        // Вимикаємо raycastTarget, щоб UI не блокував drop-подію
        if (image) image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // за курсором
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool accepted = false;

        // Якщо ми НЕ над UI, значить пробуємо дропнути предмет "у світ"
        if (EventSystem.current != null && !EventSystem.current.IsPointerOverGameObject())
        {
            if (Camera.main != null)
            {
                Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit.collider != null)
                {
                    var receiver = hit.collider.GetComponent<IItemReceiver>()
                                   ?? hit.collider.GetComponentInParent<IItem
