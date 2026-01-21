using UnityEngine;

public interface IItemReceiver
{
    bool TryAcceptItem(ItemData item, InventoryManager inventory);
}