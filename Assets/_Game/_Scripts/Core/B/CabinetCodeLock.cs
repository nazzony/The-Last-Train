using UnityEngine;

/*
 * - гравець вводить код (логіка вводу може бути будь-яка: UI/кнопки
 * - якщо код правильний → відкриваємо шафу і показуємо предмет усередині 
 * - стан зберігається (щоб не вводити код повторно)
 
 * Це ТІЛЬКИ логіка перевірки + стан. UI вводу коду сюди не зашиваю
 */

public class CabinetCodeLock : MonoBehaviour
{
    [Header("Lock Settings")]
    [SerializeField] private string cabinetId = "cabinet_01";
    [SerializeField] private string correctCode = "1200"; // тут правильний код можна поміняти так то

    [Header("Reveal / Open Result")]
    [SerializeField] private GameObject toReveal; // Wheel
    [SerializeField] private Collider2D interactCollider; // щоб більше не клікати по шафі 

    private bool opened;
    private string SaveKey => $"cabinet_opened_{cabinetId}";

    private void Awake()
    {
        if (interactCollider == null)
            interactCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        opened = PlayerPrefs.GetInt(SaveKey, 0) == 1;
        if (opened) ApplyOpenedState();
    }

    // Це викликаєш з UI вводу коду, або тимчасово з іншого скрипта
    public bool SubmitCode(string code)
    {
        if (opened) return true;
        if (string.IsNullOrEmpty(code)) return false;

        if (code != correctCode)
        {
            Debug.Log("Wrong code");
            return false;
        }

        opened = true;
        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        ApplyOpenedState();
        Debug.Log("Cabinet opened");
        return true;
    }
    
    private void ApplyOpenedState()
    {
        if (toReveal != null)
            toReveal.SetActive(true);

        if (interactCollider != null)
            interactCollider.enabled = false;
    }
}
