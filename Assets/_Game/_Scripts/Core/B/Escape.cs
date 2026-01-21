using UnityEngine;

public class Escape: MonoBehaviour, IItemReceiver
{
    private bool hasWheel;
    private bool hasLever;

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;

        switch (item.itemType)
        {
            case ItemData.ItemType.Wheel:
                if (hasWheel) { Debug.Log("Wheel already used"); return false; }
                hasWheel = true;
                inventory.RemoveItem(item);
                Debug.Log("Wheel installed.");
                return true;

            case ItemData.ItemType.Lever:
                if (hasLever) { Debug.Log("Lever already used."); return false; }
                hasLever = true;
                inventory.RemoveItem(item);
                Debug.Log("Lever used.");
                return true;

            default:
                Debug.Log("This item doesn't fit.");
                return false;
        }
    }

    public bool CanRide()
    {
        return hasWheel && hasLever;
    }
}