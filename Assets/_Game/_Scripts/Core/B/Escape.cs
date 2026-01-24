using UnityEngine;

public class Escape : MonoBehaviour, IItemReceiver
{
    private bool hasWheel;
    private bool hasLever;
    private bool rideAllowed; //перевірка чи все встановлено і можна їхати

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;

        switch (item.itemType)
        {
            case ItemData.ItemType.Wheel:
                if (hasWheel) return false;
                hasWheel = true;
                inventory.RemoveItem(item);
                Debug.Log("Wheel installed.");
                return true;

            case ItemData.ItemType.Lever:
                if (hasLever) return false;
                hasLever = true;
                inventory.RemoveItem(item);
                Debug.Log("Lever installed.");
                return true;

            default:
                return false;
        }
    }
    public bool CanRide()
    {
        return hasWheel && hasLever;
    }
    
    private void OnMouseDown() //після встановлення натиснути на дрезину шоб поїхати
    {
        if (!CanRide())
        {
            Debug.Log("Can't ride: missing parts.");
            return;
        }

        rideAllowed = true;
        Debug.Log("Ride allowed. (Animation/scene transition is handled by another system)");
    }
    
    public bool IsRideAllowed()
    {
        return rideAllowed;
    }
}