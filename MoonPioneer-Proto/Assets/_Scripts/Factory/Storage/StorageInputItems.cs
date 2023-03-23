using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class StorageInputItems : StorageBase 
{
    [SerializeField] private Item.TypeItem[] typeItems = new Item.TypeItem[0];

    public override bool IsCanAddItem(GameObject objItem) 
    {
        return base.IsCanAddItem(objItem) && objItem.GetComponent<Item>() is Item item && typeItems.Contains(item.type);
    }

    protected override void MoveItem() 
    {
        GameObject objItem = _inventoryInTrigger.GetLastItem(typeItems);

        if (objItem != null && IsCanAddItem(objItem)) 
        {
            _inventoryInTrigger.RemoveItem(objItem);
            AddItem(objItem);
        }
    }
}
