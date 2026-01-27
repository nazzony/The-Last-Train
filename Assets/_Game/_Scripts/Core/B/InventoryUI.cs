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
    public InventoryManager inventory;
    
    public Transform slotsParent;
    
    public GameObject itemIconPrefab;
    
    private InventorySlot[] slots;

    private void Awake()
    {
        if (slotsParent == null)
        {
            Debug.LogError("InventoryUI: slotsParent не призначений");
            slots = new InventorySlot[0];
            return;
        }

        slots = slotsParent.GetComponentsInChildren<InventorySlot>(true);
    }
    private void Start()
    {
        inventory = InventoryManager.instance;

        if (inventory != null)
        {
            inventory.OnInventoryChanged -= Refresh;
            inventory.OnInventoryChanged += Refresh;

            Refresh();
        }
        else
        {
            Debug.LogError("InventoryUI: SOS! Не можу знайти InventoryManager.instance!");
        }
    }

    private void OnEnable()
    {
        if (inventory != null)
        {
            inventory.OnInventoryChanged += Refresh;
            Refresh();
        }
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }

    public void Refresh()
    {
        Debug.Log("InventoryUI: Refresh спрацював!");

        if (inventory == null || slots == null) return;

        if (itemIconPrefab == null)
        {
            Debug.LogError("InventoryUI: itemIconPrefab не призначений");
            return;
        }

        foreach (var slot in slots)
        {
            for (int i = slot.transform.childCount - 1; i >= 0; i--)
            {
                Destroy(slot.transform.GetChild(i).gameObject);
            }
        }

        //int count = Mathf.Min(inventory.items.Count, slots.Length);

        for (int i = 0; i < inventory.items.Count; i++)
        {
            ItemData data = inventory.items[i];
            if (data == null) continue;

            GameObject icon = Instantiate(itemIconPrefab, slots[i].transform);

            var img = icon.GetComponent<Image>();
            if (img != null)
                img.sprite = data.icon;

            var drag = icon.GetComponent<DraggableItem>();
            if (drag != null)
            {
                drag.itemData = data;

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
