/*
 * Основна логіка інвентаря:
 * - підбір предметів зі світу (через тригер 2D)
 * - зберігання предметів у списку
 * - видалення предметів після використання
 * - пошук предметів за типом (для пазлів)
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //Singleton

    public static InventoryManager instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //Main Script

    [SerializeField] private int maxSize = 10;

    public readonly List<ItemData> items = new List<ItemData>();

    public event Action OnInventoryChanged;

    public bool HasSpace() => items.Count < maxSize;

    public bool AddItem(ItemData item)
    {
        if (item == null) return false;
        if (!HasSpace()) return false;

        items.Add(item);
        OnInventoryChanged?.Invoke();
        return true;
    }

    public bool RemoveItem(ItemData item)
    {
        if (item == null) return false;

        bool removed = items.Remove(item);
        if (removed) OnInventoryChanged?.Invoke();
        return removed;
    }

    public bool HasItemType(ItemData.ItemType type)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].itemType == type) return true;
        return false;
    }

    public ItemData FindFirstByType(ItemData.ItemType type)
    {
        for (int i = 0; i < items.Count; i++)
            if (items[i].itemType == type) return items[i];
        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var container = other.GetComponent<InstanceItemContainer>();
        if (container == null) return;

        if (container.item == null) return;

        if (!HasSpace())
        {
            // інвентар повний — нічого не забираємо
            return;
        }

        ItemData taken = container.TakeItem();
        if (taken == null) return;

        AddItem(taken);
        // якщо предмет у світі має зникнути:
        Destroy(other.gameObject);
    }
}
