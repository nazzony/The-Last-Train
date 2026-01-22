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

    public void RemoveItem(ItemData itemToRemove)
    {
        if (itemToRemove == null) return;

        if (items.Remove(itemToRemove))
        {
            OnInventoryChanged?.Invoke();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out InstanceItemContainer foundItem))
        {
            ItemData data = foundItem.TakeItem();
            bool added = AddItem(data);

            if (added)
            {
                Destroy(foundItem.gameObject);
            }
        }
    }
}
