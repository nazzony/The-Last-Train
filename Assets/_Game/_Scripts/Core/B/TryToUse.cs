using UnityEngine;

public class TryToUse : MonoBehaviour, IItemReceiver
{
    public ItemData keyItem;
    public InventoryManager inventory;
    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;
        if (item.itemType != ItemData.ItemType.Coin) return false;

        return ExchangeCoinForKey(inventory, item);
    }
    private void OnMouseDown() //якщо гравець не пертягне монету а клікне по автомату
    {
        if (inventory == null) return;
        
        ItemData coin = FindFirstCoin(inventory);
        if (coin == null) return;

        ExchangeCoinForKey(inventory, coin);
    }
    
    private bool ExchangeCoinForKey(InventoryManager inv, ItemData coin) //логіка зміни монети на ключ
    {
        if (keyItem == null) return false;
        inv.RemoveItem(coin);
        
        if (!inv.AddItem(keyItem))
        {
            inv.AddItem(coin); //якщо ключ не вліз то не додаєм, але по факту не дуже потрібне бо це -1+1, але хай буде на випадок багу
            return false;
        }

        Debug.Log("Coin accepted, key given!");
        return true;
    }

    private ItemData FindFirstCoin(InventoryManager inv)
    {
        for (int i = 0; i < inv.items.Count; i++)
        {
            var it = inv.items[i];
            if (it != null && it.itemType == ItemData.ItemType.Coin)
                return it;
        }
        return null;
    }
}