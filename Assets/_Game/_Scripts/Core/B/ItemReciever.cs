// Receivers handle item consumption/rewards themselves.
// Return true if item was accepted (and inventory/quest state updated).
public interface IItemReceiver
{
    bool TryAcceptItem(ItemData item, InventoryManager inventory);
}