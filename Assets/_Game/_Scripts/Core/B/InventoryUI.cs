using UnityEngine;
using UnityEngine.UI;

/*
 * Відповідає ТІЛЬКИ за відображення інвентаря.
 * Логіка:
 * - слухає InventoryManager (подія OnInventoryChanged)
 * - при зміні інвентаря перемальовує всі слоти
 * - створює UI-іконки предметів
 * - передає дані в DraggableItem для drag & drop
 */

public class InventoryUI : MonoBehaviour
{
    // Посилання на менеджер інвентаря (де зберігаються предмети)
    public InventoryManager inventory;
    
    public Transform slotsParent;
    
    public GameObject itemIconPrefab;
    
    private InventorySlot[] slots;

    private void Awake()
    {
        // Захист від помилки, якщо slotsParent не призначили в інспекторі
        if (slotsParent == null)
        {
            Debug.LogError("InventoryUI: slotsParent не призначений");
            slots = new InventorySlot[0];
            return;
        }

        // Знаходимо всі InventorySlot серед дочірніх обʼєктів
        slots = slotsParent.GetComponentsInChildren<InventorySlot>(true);
    }

    private void OnEnable()
    {
        // Підписуємось на подію зміни інвентаря
        if (inventory != null)
        {
            inventory.OnInventoryChanged += Refresh;
            Refresh();
        }
    }

    private void OnDisable()
    {
        // Відписуємось, щоб не було витоків і подвійних викликів
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }

    // Оновлення відображення інвентаря
    public void Refresh()
    {
        if (inventory == null || slots == null) return;

        if (itemIconPrefab == null)
        {
            Debug.LogError("InventoryUI: itemIconPrefab не призначений");
            return;
        }

        // 1) Очищаємо всі слоти від старих іконок
        foreach (var slot in slots)
        {
            for (int i = slot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(slot.transform.GetChild(i).gameObject);
            }
        }

        // 2) Створюємо нові іконки відповідно до inventory.items
        int count = Mathf.Min(inventory.items.Count, slots.Length);

        for (int i = 0; i < count; i++)
        {
            ItemData data = inventory.items[i];
            if (data == null) continue;

            // Створюємо іконку предмета в слоті
            GameObject icon = Instantiate(itemIconPrefab, slots[i].transform);

            // Призначаємо спрайт предмета
            var img = icon.GetComponent<Image>();
            if (img != null)
                img.sprite = data.icon;

            // Налаштовуємо drag & drop
            var drag = icon.GetComponent<DraggableItem>();
            if (drag != null)
            {
                // Дані предмета
                drag.itemData = data;

                // Посилання на інвентар (щоб можна було витрачати предмет)
                drag.inventory = inventory;
                
                if (drag.image == null)
                    drag.image = img;
                drag.parentAfterDrag = slots[i].transform;
            }
            else
            {
                Debug.LogError("InventoryUI: у itemIconPrefab відсутній DraggableItem");
            }
        }
    }
}
