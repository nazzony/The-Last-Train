using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public InventoryManager inventory;
    public Transform slotsParent;        
    public GameObject itemIconPrefab;    

    private InventorySlot[] slots;

    private void Awake()
    {
        slots = slotsParent.GetComponentsInChildren<InventorySlot>(true);
    }

    private void OnEnable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged += Refresh;
    }

    private void OnDisable()
    {
        if (inventory != null)
            inventory.OnInventoryChanged -= Refresh;
    }

    public void Refresh()
    {
        foreach (var s in slots)
        {
            for (int i = s.transform.childCount - 1; i >= 0; i--)
                Destroy(s.transform.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < inventory.items.Count && i < slots.Length; i++)
        {
            ItemData data = inventory.items[i];

            GameObject icon = Instantiate(itemIconPrefab, slots[i].transform);
            var img = icon.GetComponent<Image>();
            var drag = icon.GetComponent<DraggableItem>();

            if (img != null) img.sprite = data.icon; 
            if (drag != null)
            {
                drag.itemData = data;
                drag.inventory = inventory;
                if (drag.image == null) drag.image = img;
                drag.parentAfterDrag = slots[i].transform;
            }
        }
    }
}