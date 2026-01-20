using UnityEngine;

public class TryToUse: MonoBehaviour
{
    public ItemData keyItem; 
    public bool TryUse(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;
        if (item.itemType != ItemData.ItemType.Coin) return false;

        inventory.RemoveItem(item);     
        inventory.AddItem(keyItem);     
        Debug.Log("Key added!");

        return true;
    }

    public bool Escape(ItemData item, InventoryManager inventory, ItemData otherItem)
    {
        if (item == null || inventory == null) return false;
        if (item.itemType == ItemData.ItemType.Lever && otherItem.itemType == ItemData.ItemType.Wheel)
        {
            inventory.RemoveItem(item);
            inventory.RemoveItem(otherItem);
            Debug.Log("Escaped");
            return true;
        }
        return false;
    }
}