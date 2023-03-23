using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int limitItems = 10;
    
    [SerializeField] private Vector3 posFirstItem = Vector3.zero;
    [SerializeField] private Vector3 offsetPosItem = Vector3.zero;
    [SerializeField] private Vector3 rotationItem = Vector3.zero;

    private List<GameObject> allObjectsItems = new List<GameObject>();

    public int countItems { get { return allObjectsItems.Count; } }
    
    public virtual bool IsCanAddItem(GameObject objItem) 
    {
        Item item = objItem.GetComponent<Item>();
        if (item == null) return false;

        MoverGameObject move = objItem.GetComponent<MoverGameObject>();
        if (move == null) return false;

        if (allObjectsItems.Count >= limitItems) return false;

        return true;
    }

    public bool AddItem(GameObject objItem) 
    {
        Item item = objItem.GetComponent<Item>();
        if (item == null) return false;

        MoverGameObject move = objItem.GetComponent<MoverGameObject>();
        if (move == null) return false;

        int index = allObjectsItems.Count;
        allObjectsItems.Add(objItem);
        objItem.transform.SetParent(gameObject.transform);

        MoveItemToIndexPos(move, index);

        return true;
    }

    public GameObject GetLastItem(Item.TypeItem typeItem) => GetLastItem(new Item.TypeItem[]{typeItem});

    public GameObject GetLastItem(Item.TypeItem[] typesItems) 
    {
        for(int i = allObjectsItems.Count - 1; i >= 0; --i) 
        {
            GameObject objItem = allObjectsItems[i];
            Item item = objItem.GetComponent<Item>();
            if(Array.IndexOf(typesItems, (item.type)) != -1) return objItem;
        }

        return null;
    }

    public GameObject GetLastItem() 
    {
        int lastIndex = allObjectsItems.Count - 1;

        if (lastIndex < 0) return null;
        GameObject objItem = allObjectsItems[lastIndex];
        return objItem;
    }

    public void RemoveItem(GameObject objItem) 
    {
        allObjectsItems.Remove(objItem);
        UpdatePosItems();
    }

     public void UpdatePosItems() 
     {
        for (int i = 0; i < allObjectsItems.Count; ++i) 
        {
            GameObject objItem = allObjectsItems[i];

            MoverGameObject move = objItem.GetComponent<MoverGameObject>();
            MoveItemToIndexPos(move, i);
        }
    }

    private void MoveItemToIndexPos(MoverGameObject move, int index) 
    {
        move.MoveTo(posFirstItem + (offsetPosItem * index), rotationItem);
    }
}
