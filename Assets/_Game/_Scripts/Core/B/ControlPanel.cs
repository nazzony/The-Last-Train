using UnityEngine;

/*
 * Пазл панелі керування біля дрезини 
 * - приймає SmallKey: відкриває панель (unlock)
 * - приймає Battery: вставляє батарейку
 * - коли панель відкрита + батарейка вставлена → PowerReady = true
 * - стан зберігається 
 */

public class ControlPanel : MonoBehaviour, IItemReceiver
{
    [Header("Persistence")]
    [SerializeField] private string panelId = "panel_01";

    [Header("Visuals (Optional)")]
    [SerializeField] private GameObject panelOpenedVisual;  // що показати, коли панель відкрита
    [SerializeField] private GameObject batteryInstalledVisual; // що показати, коли батарейку вставили

    private bool unlocked;
    private bool batteryInstalled;

    private string UnlockKey => $"panel_unlocked_{panelId}";
    private string BatteryKey => $"panel_battery_{panelId}";

    public bool PowerReady => unlocked && batteryInstalled;

    private void Start()
    {
        unlocked = PlayerPrefs.GetInt(UnlockKey, 0) == 1;
        batteryInstalled = PlayerPrefs.GetInt(BatteryKey, 0) == 1;

        ApplyState();
    }

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;

        // Відкрити панель маленьким ключиком
        if (item.itemType == ItemData.ItemType.SmallKey)
        {
            if (unlocked) return false;

            unlocked = true;
            inventory.RemoveItem(item);

            PlayerPrefs.SetInt(UnlockKey, 1);
            PlayerPrefs.Save();

            ApplyState();
            Debug.Log("Panel unlocked");
            return true;
        }

        // Вставити батарейку (тільки якщо панель вже відкрита)
        if (item.itemType == ItemData.ItemType.Battery)
        {
            if (!unlocked) return false;
            if (batteryInstalled) return false;

            batteryInstalled = true;
            inventory.RemoveItem(item);

            PlayerPrefs.SetInt(BatteryKey, 1);
            PlayerPrefs.Save();

            ApplyState();
            Debug.Log("Battery installed");
            return true;
        }

        return false;
    }

    private void ApplyState()
    {
        if (panelOpenedVisual != null)
            panelOpenedVisual.SetActive(unlocked);

        if (batteryInstalledVisual != null)
            batteryInstalledVisual.SetActive(batteryInstalled);
    }
}
