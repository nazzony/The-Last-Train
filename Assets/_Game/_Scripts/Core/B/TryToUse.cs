using UnityEngine;

public class TryToUse : MonoBehaviour, IItemReceiver
{
    public ItemData keyItem;

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;
        if (item.itemType != ItemData.ItemType.Coin) return false;

        inventory.RemoveItem(item);
        inventory.AddItem(keyItem);

        Debug.Log("Coin accepted, key given!");
        return true;
    }
}