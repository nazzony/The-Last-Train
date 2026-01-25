using UnityEngine;
using UnityEngine.Events;
//логіка для потрапляння в дері вокзалу з втратою ключа але збереженням стану дверей(відкриті)
public class DoorLock : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private InventoryManager inventory;
    [SerializeField] private ItemData.ItemType requiredKey = ItemData.ItemType.Key;

    [Header("Persistence")]
    [SerializeField] private string doorId = "station_door_01"; 

    [Header("Door Colliders")] 
    [SerializeField] private Collider blockingCollider;

    [SerializeField] private Collider interactCollider;

    [Header("Events")]
    public UnityEvent onOpened;
    public UnityEvent onLocked;

    private bool opened;

    private string SaveKey => $"door_opened_{doorId}";

    private void Awake()
    {
      
        // Якщо в об'єкта 2 колайдери (один блокує, інший для кліку) — підхопимо їх автоматично
        if (blockingCollider == null || interactCollider == null)
        {
            var cols = GetComponents<Collider>();
            if (cols != null && cols.Length > 0)
            {
                if (blockingCollider == null) blockingCollider = cols[0];
                if (interactCollider == null) interactCollider = (cols.Length > 1) ? cols[1] : cols[0];
            }
        }
    }

    private void Start()
    {
      
        opened = PlayerPrefs.GetInt(SaveKey, 0) == 1;

        if (opened)
            ApplyOpenedState();
    }

    private void OnMouseDown()
    {
        TryOpen();
    }

    public void TryOpen()
    {
        if (opened) return;
        if (inventory == null) return;

        if (!inventory.HasItemType(requiredKey))
        {
            Debug.Log("No key");
            onLocked?.Invoke();
            return;
        }

        // 1) Витратити ключ 
        var keyItem = inventory.FindFirstByType(requiredKey);
        if (keyItem == null)
        {
            Debug.Log("No key");
            onLocked?.Invoke();
            return;
        }
        inventory.RemoveItem(keyItem);

        // 2) Позначити як відкриті та зберегти
        opened = true;
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        ApplyOpenedState();

        Debug.Log("Door opened");
        onOpened?.Invoke();
    }

    private void ApplyOpenedState()
    {
        if (blockingCollider != null) blockingCollider.enabled = false;

        // Щоб більше не клікати по дверях (і не ловити баги)
        if (interactCollider != null) interactCollider.enabled = false;
    }
}
