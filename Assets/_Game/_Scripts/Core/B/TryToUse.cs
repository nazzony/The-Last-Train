using UnityEngine;

public class TryToUse : MonoBehaviour, IItemReceiver
{
    public ItemData keyItem;

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;
        if (item.itemType != ItemData.ItemType.Coin) return false;

        // Спочатку перевіряємо, чи є місце під ключ (бо монету ми заберемо)
        if (!inventory.HasSpace(0))
        {
            
        }

        // Забираємо монету
        inventory.RemoveItem(item);

        // Даємо ключ. Якщо не влазить - повертаємо монету назад.
        if (!inventory.AddItem(keyItem))
        {
            inventory.AddItem(item);
            return false;
        }

        Debug.Log("Coin accepted, key given!");
        return true;
    }
}