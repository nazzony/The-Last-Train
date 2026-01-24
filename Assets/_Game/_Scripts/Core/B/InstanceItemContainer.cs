using UnityEngine;

public class InstanceItemContainer : MonoBehaviour
{
    public ItemData item;

    public ItemData TakeItem()
    {
        var taken = item;
        item = null;
        return taken;
    }
}