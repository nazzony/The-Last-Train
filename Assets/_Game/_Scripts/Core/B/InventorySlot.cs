using UnityEngine;
using UnityEngine.EventSystems;

/*
 * Один слот інвентаря (UI).
 *
 * Відповідає ТІЛЬКИ за:
 * - прийом іконки предмета при перетягуванні (Drag & Drop)
 * - визначення, чи слот порожній
 * ВАЖЛИВО:
 * - НЕ змінює inventory.items
 * - НЕ додає і НЕ видаляє предмети
 * - працює лише з UI-іконками
 */

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        // Якщо слот вже зайнятий — нічого не приймаємо
        if (transform.childCount != 0) return;

        // Обʼєкт, який перетягують (іконка предмета)
        GameObject dropped = eventData.pointerDrag;
        if (dropped == null) return;

        // Отримуємо компонент DraggableItem з іконки
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if (draggableItem == null) return;
        draggableItem.parentAfterDrag = transform;
    }
}
