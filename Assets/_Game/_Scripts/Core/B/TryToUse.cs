using UnityEngine;

public class TryToUse : MonoBehaviour, IItemReceiver
{
    public ItemData keyItem;
    public InventoryManager inventory;

    [Header("One-time use")]
    public string machineId = "machine_01";
    private bool used;
    private string SaveKey => $"machine_used_{machineId}";

    private void Start()
    {
        used = PlayerPrefs.GetInt(SaveKey, 0) == 1;
    }

    public bool TryAcceptItem(ItemData item, InventoryManager inv)
    {
        if (used) return false;
        if (item == null || inv == null) return false;
        if (item.itemType != ItemData.ItemType.Coin) return false;

        return ExchangeCoinForKey(inv, item);
    }

    private void OnMouseDown() //якщо гравець не пертягне монету а клікне по автомату
    {
        if (used) return;
        if (inventory == null) return;

        ItemData coin = inventory.FindFirstByType(ItemData.ItemType.Coin);
        if (coin == null) return;

        ExchangeCoinForKey(inventory, coin);
    }

    private bool ExchangeCoinForKey(InventoryManager inv, ItemData coin) //логіка зміни монети на ключ
    {
        if (keyItem == null) return false;

        // витрачаємо монету
        if (!inv.RemoveItem(coin)) return false;

        // додаємо ключ
        if (!inv.AddItem(keyItem))
        {
            // якщо ключ не вліз то повертаємо монету назад
            inv.AddItem(coin);
            return false;
        }
        used = true;
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log("Coin accepted, key given!");
        return true;
    }
}