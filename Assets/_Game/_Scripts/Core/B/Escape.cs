using UnityEngine;

/*
 * Escape
 * ------
 * Відповідає за фінальну логіку дрезини.
 *
 * Функціонал:
 * - приймає деталі дрезини (колесо, важіль) через drag & drop з інвентаря
 * - зберігає стан встановлених деталей
 * - перевіряє, чи всі необхідні частини встановлені
 * - після кліку на дрезину дозволяє почати поїздку
 *
 * Скрипт використовується як puzzle-перевірка перед фінальним етапом гри.
 */

public class Escape : MonoBehaviour, IItemReceiver
{
    [Header("Persistence")]
    [SerializeField] private string trolleyId = "trolley_01";

    private bool hasWheel;
    private bool hasLever;
    private bool rideAllowed; //перевірка чи все встановлено і можна їхати

    private string WheelKey => $"trolley_wheel_{trolleyId}";
    private string LeverKey => $"trolley_lever_{trolleyId}";
    private string RideKey  => $"trolley_ride_{trolleyId}";
    
    [SerializeField] private ControlPanel controlPanel;


    private void Start()
    {
        // відновлюємо стан дрезини після перезавантаження сцени
        hasWheel = PlayerPrefs.GetInt(WheelKey, 0) == 1;
        hasLever = PlayerPrefs.GetInt(LeverKey, 0) == 1;
        rideAllowed = PlayerPrefs.GetInt(RideKey, 0) == 1;
    }

    public bool TryAcceptItem(ItemData item, InventoryManager inventory)
    {
        if (item == null || inventory == null) return false;

        switch (item.itemType)
        {
            case ItemData.ItemType.Wheel:
                if (hasWheel) return false;
                hasWheel = true;
                inventory.RemoveItem(item);
                PlayerPrefs.SetInt(WheelKey, 1);
                PlayerPrefs.Save();
                Debug.Log("Wheel installed.");
                return true;

            case ItemData.ItemType.Lever:
                if (hasLever) return false;
                hasLever = true;
                inventory.RemoveItem(item);
                PlayerPrefs.SetInt(LeverKey, 1);
                PlayerPrefs.Save();
                Debug.Log("Lever installed.");
                return true;
        }
        return false;
    }

    public bool CanRide()
    {
        bool partsOk = hasWheel && hasLever;
        bool powerOk = (controlPanel == null) ? true : controlPanel.PowerReady; 
        return partsOk && powerOk;
    }


    private void OnMouseDown() //після встановлення натиснути на дрезину шоб поїхати
    {
        if (!CanRide())
        {
            Debug.Log("Can't ride: missing parts.");
            return;
        }

        rideAllowed = true;
        PlayerPrefs.SetInt(RideKey, 1);
        PlayerPrefs.Save();

        Debug.Log("Ride allowed");
    }

    public bool IsRideAllowed()
    {
        return rideAllowed;
    }
}
