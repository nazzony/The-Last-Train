using UnityEngine;

public class TryToUse : MonoBehaviour, IItemReceiver
{
    public ItemData keyItem;

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;
        if (item.itemType != ItemData.ItemType.Coin) return false;
        
        if (!inventory.HasSpace(0))
        { }
        
        inventory.RemoveItem(item);
        
        if (!inventory.AddItem(keyItem))
        {
            inventory.AddItem(item);
            return false;
        }

        Debug.Log("Coin accepted, key given!");
        return true;
    }
}