using UnityEngine;

public class InstanceItemContainer : MonoBehaviour
{
    public ItemData itemData;
    
    public ItemData TakeItem()
    {
        return itemData;
    }
public interface IItemReceiver
{
    bool TryAcceptItem(ItemData item, InventoryManager inventory);
}

}