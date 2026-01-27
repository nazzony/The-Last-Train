// 1. Додаємо IItemReceiver через кому
using UnityEngine;

public class TryToUse : MonoBehaviour, IItemReceiver
{
    [Header("Settings")]
    public ItemData keyItem;
    public ItemData coinItem;

    [Header("Saving")]
    public string machineId = "machine_01";
    private bool used;
    private string SaveKey => $"machine_used_{machineId}";

    private void Start()
    {
        used = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        if (used) Debug.Log($"Machine {machineId} is already empty.");
    }

    // 2. Реалізація контракту IItemReceiver
    // Цей метод викликає InputManager
    public bool TryAcceptItem(ItemData item, InventoryManager inv)
    {
        if (used)
        {
            Debug.Log("Автомат вже пустий.");
            AudioManager.instance.playSFX(AudioManager.instance.clickSound);
            return false;
        }

        // ХИТРІСТЬ: Якщо ми клікнули (item == null), ми самі шукаємо монету в кишені
        ItemData itemToUse = item;
        if (itemToUse == null)
        {
            itemToUse = inv.FindFirstByType(coinItem.itemType);
        }

        // Якщо монети немає ні в руці, ні в кишені
        if (itemToUse == null)
        {
            Debug.Log("Потрібна монета!");
            AudioManager.instance.playSFX(AudioManager.instance.clickSound);
            return false;
        }

        // Якщо це не той тип предмета (наприклад, пхають ключ замість монети)
        if (itemToUse.itemType != coinItem.itemType)
        {
            Debug.Log("Цей предмет сюди не підходить.");
            return false;
        }

        // 3. Обмін
        // Видаляємо монету
        inv.RemoveItem(itemToUse);
        // Даємо ключ
        inv.AddItem(keyItem);

        // Зберігаємо
        used = true;
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        // Ефекти
        Debug.Log("Coin accepted, key given!");
        AudioManager.instance.playSFX(AudioManager.instance.coinSound);

        return true;
    }
}