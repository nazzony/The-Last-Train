using UnityEngine;

public class InstanceItemContainer : MonoBehaviour
{
    public ItemData item;

    public ItemData TakeItem()
    {
        return item;
    }
}