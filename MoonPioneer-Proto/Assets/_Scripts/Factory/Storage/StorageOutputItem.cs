using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageOutputItem : StorageBase 
{
    protected override void MoveItem() 
    {
        GameObject item = GetLastItem();

        if (item != null && _inventoryInTrigger.IsCanAddItem(item)) 
        {
            RemoveItem(item);
            _inventoryInTrigger.AddItem(item);
        }
    }
}
