using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/*
 * Логіка:
 * - під час перетягування іконка слідує за курсором
 * - якщо предмет відпустили над UI → іконка повертається в слот
 * - якщо предмет відпустили НЕ над UI:
 *     * позиція курсору переводиться з Screen space у World space через камеру
 *     * виконується Physics2D.Raycast у точці курсору
 *     * шукається обʼєкт, який реалізує IItemReceiver
 *     * предмет передається цьому обʼєкту
 *
 * Якщо предмет прийнято:
 * - іконка видаляється з UI
 * - receiver сам вирішує, що робити з предметом (витратити / прийняти)
 *
 * ВАЖЛИВО:
 * - працює для 2D сцен
 * - потребує EventSystem у сцені
 * - камера має дивитись на сцену (Orthographic або Perspective)
 */

public class DraggableItem : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // Image іконки предмета
    public Image image;

    // Батьківський обʼєкт, куди іконка повертається, якщо дроп не прийняли
    [HideInInspector] public Transform parentAfterDrag;

    [Header("Item Data")]
    // Дані предмета (ScriptableObject)
    public ItemData itemData;

    // Посилання на інвентар (щоб receiver міг витратити предмет)
    public InventoryManager inventory;
    
    [Header("Camera")]
    public Camera cam;

    private void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Запамʼятовуємо слот, з якого почали drag
        parentAfterDrag = transform.parent;
        
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

        // Вимикаємо raycastTarget, щоб UI не блокував drop
        if (image != null)
            image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Іконка рухається за курсором
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool accepted = false;
        
        if (EventSystem.current != null &&
            !EventSystem.current.IsPointerOverGameObject())
        {
            if (cam != null)
            {
                // Переводимо позицію миші з екрану в координати світу
                Vector2 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
                
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

                if (hit.collider != null)
                {
                    var receiver = hit.collider.GetComponent<IItemReceiver>()
                                   ?? hit.collider.GetComponentInParent<IItemReceiver>();

                    if (receiver != null)
                    {
                        accepted = receiver.TryAcceptItem(itemData, inventory);
                    }
                }
            }
            else
            {
                Debug.LogWarning("DraggableItem: Camera not assigned");
            }
        }

        // Якщо предмет прийняли — видаляємо іконку з UI
        if (accepted)
        {
            Destroy(gameObject);
            return;
        }
        
        transform.SetParent(parentAfterDrag);
        transform.localPosition = Vector3.zero;
        
        if (image != null)
            image.raycastTarget = true;
    }
}
