using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public List<ItemData> items = new();
    public int MaxSize = 10;            
    
    public bool AddItem(ItemData itemToAdd)
    {
        if (items.Count < MaxSize)
        {
            items.Add(itemToAdd);
            Debug.Log($"Picked up: {itemToAdd.itemName}");
            return true;
        }

        Debug.Log("Inventory is full!");
        return false;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out InstanceItemContainer foundItem))
        {
            bool added = AddItem(foundItem.TakeItem());
            if (added)
            {
                Destroy(foundItem.gameObject);
            }
        }
    }
    //public void Using() //in future remote to class main
    //{
      //  Debug.Log("Using Item: " + itemName);
    //}

    public void RemoveItem(ItemData itemToRemove)
    {
        if (items.Contains(itemToRemove))
            items.Remove(itemToRemove);
    }
}