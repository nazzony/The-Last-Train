/*
 * Описує один предмет гри як дані
 * - назва
 * - іконка для інвентаря
 * - тип (для перевірок у пазлах)
 */

using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public enum ItemType
    {
        Coin,
        Key,       
        SmallKey,   
        Lever,      
        Wheel,      
        Battery
    }
}

    [Header("Info")]
    public string itemName;

    [Header("UI")]
    public Sprite icon;

    [Header("Logic")]
    public ItemType itemType;
}
