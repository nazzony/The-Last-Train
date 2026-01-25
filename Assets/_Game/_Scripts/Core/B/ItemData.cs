using UnityEngine;

/*
 * ItemData
 * Використовується для:
 * - інвентаря
 * - drag & drop
 * - пазлів (перевірка типу предмета)
 */

[CreateAssetMenu(
    fileName = "NewItem",
    menuName = "Game/Item"
)]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;
    public enum ItemType
    {
        Coin,
        Key,
        SmallKey,
        Wheel,
        Lever,
        Battery
    }

}