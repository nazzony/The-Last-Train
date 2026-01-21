using UnityEngine;

public class Escape : MonoBehaviour, IItemReceiver
{
    private bool hasWheel;
    private bool hasLever;

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;

        switch (item.itemType)
        {
            case ItemData.ItemType.Wheel:
                if (hasWheel) return false;
                hasWheel = true;
                inventory.RemoveItem(item);
                return true;

            case ItemData.ItemType.Lever:
                if (hasLever) return false;
                hasLever = true;
                inventory.RemoveItem(item);
                return true;

            default:
                return false;
        }
    }

    public bool CanRide()
    {
        return hasWheel && hasLever;
    }
}