using UnityEngine;

public class InstanceItemContainer : MonoBehaviour
{
    public ItemData itemData;
    
    public ItemData TakeItem()
    {
        return itemData;
    }

}