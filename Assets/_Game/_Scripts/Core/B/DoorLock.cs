using UnityEngine;
using UnityEngine.Events;

//логіка для потрапляння в дері вокзалу з втратою ключа але збереженням стану дверей(відкриті)
/*
 * Пазл-двері вокзалу:
 * - при кліку перевіряє, чи є потрібний ключ в інвентарі
 * - якщо ключ є → витрачає його, відкриває двері, зберігає стан (PlayerPrefs)
 * - якщо ключа немає → викликає onLocked

 * Колайдери:
 * - blockingCollider: блокує прохід (вимикаємо після відкриття)
 * - interactCollider: ловить клік (вимикаємо після відкриття, щоб не клікали знову)
 */

public class DoorLock : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private ItemData.ItemType requiredKey = ItemData.ItemType.Key;

    [Header("Persistence")]
    [SerializeField] private string doorId = "station_door_01";

    [Header("Door Colliders")]
    [SerializeField] private Collider2D blockingCollider;
    [SerializeField] private Collider2D interactCollider;

    [Header("Events")]
    public UnityEvent onOpened;
    public UnityEvent onLocked;

    private bool opened;
    private string SaveKey => $"door_opened_{doorId}";

    private void Awake()
    {
        
        if (blockingCollider == null || interactCollider == null)
        {
            var cols = GetComponents<Collider2D>();

            if (cols.Length > 0)
            {
                if (blockingCollider == null) blockingCollider = cols[0];

                // Якщо є другий колайдер — беремо його як interact
                if (interactCollider == null)
                    interactCollider = (cols.Length > 1) ? cols[1] : cols[0];
            }
        }

       
    }

    private void Start()
    {
        opened = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        if (opened) ApplyOpenedState();
    }

    private void OnMouseDown()
    {
        TryOpen();
    }

    public void TryOpen()
    {
        if (opened || inventory == null) return;

        var key = inventory.FindFirstByType(requiredKey);
        if (key == null)
        {
            Debug.Log("No key");
            onLocked?.Invoke();
            return;
        }
        inventory.RemoveItem(key);

        // зберігаємо стан "відкрито"
        opened = true;
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        ApplyOpenedState();
        onOpened?.Invoke();
    }

    private void ApplyOpenedState()
    {
        // Блокуючий колайдер вимикаємо завжди (щоб можна пройти)
        if (blockingCollider != null) blockingCollider.enabled = false;
        // Колайдер для кліку вимикаємо лише якщо він інший,
        if (interactCollider != null && interactCollider != blockingCollider)
            interactCollider.enabled = false;
    }
}
