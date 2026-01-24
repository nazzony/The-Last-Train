using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStack = 1;
    public GameObject worldPrefab;
    public ItemType itemType;

    [TextArea]
    public string description;

    public void Using()
    {
        Debug.Log("Using Item: " + itemName);
    }

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