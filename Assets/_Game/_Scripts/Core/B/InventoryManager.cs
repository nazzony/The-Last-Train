using UnityEngine;
using System;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> items = new();
    public int MaxSize = 10;

    public event Action OnInventoryChanged;

    public bool HasSpace(int count = 1)
    {
        return items.Count + count <= MaxSize;
    }

    public bool AddItem(ItemData itemToAdd)
    {
        if (itemToAdd == null)
        {
            Debug.LogWarning("Tried to add null item.");
            return false;
        }

        if (items.Count < MaxSize)
        {
            items.Add(itemToAdd);
            Debug.Log($"Picked up: {itemToAdd.itemName}");
            OnInventoryChanged?.Invoke();
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }

    public bool RemoveItem(ItemData itemToRemove)
    {
        if (itemToRemove == null) return false;

        bool removed = items.Remove(itemToRemove);
        if (removed)
            OnInventoryChanged?.Invoke();

        return removed;
    }
    public ItemData FindFirstByType(ItemData.ItemType type)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var it = items[i];
            if (it != null && it.itemType == type)
                return it;
        }
        return null;
    }

    public bool HasItemType(ItemData.ItemType type)
    {
        return FindFirstByType(type) != null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out InstanceItemContainer foundItem))
        {
            ItemData data = foundItem.TakeItem();
            if (data == null) return;

            bool added = AddItem(data);
            if (added)
            {
                Destroy(foundItem.gameObject);
            }
            else
            {
                foundItem.item = data; 
            }
        }
    }
}
