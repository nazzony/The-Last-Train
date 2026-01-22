using UnityEngine;

[CreateAssetMenu(fileName = "NewItemData", menuName = "Inventory/Item Data")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public int maxStack = 1;
    public GameObject worldPrefab;
    public ItemType itemType;

    public void Using()
    {
        Debug.Log("Using Item: " + itemName);
    }
    
    [TextArea]
    public string description;
    public enum ItemType
    {
        Coin,
        Key,
        TrainPart,
    }
}