using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Factory : MonoBehaviour 
{
    public enum ReasonStop
    { 
        NoResource, 
        FullStorage 
    }

    private bool isWork = false;

    [Header("Created Item")]
    [SerializeField] private GameObject _createdItem = null;
    [SerializeField] private Vector3 _createdItemPos = Vector3.zero;
    [SerializeField] private Vector3 _createdItemRotation = Vector3.zero;

    [Header("Storage")]
    [SerializeField] private StorageBase _inputItems = null;
    [SerializeField] private StorageBase _outputItems = null;

    [Header("Process create")]
    [SerializeField] private Vector3 _removedPosItem = Vector3.zero;
    [SerializeField] private float _timeCreatedItem = 10f;
    [SerializeField] private Item.TypeItem[] _receipt = null;

    [HideInInspector] public UnityEvent<ReasonStop, Item.TypeItem> onStop = new UnityEvent<ReasonStop, Item.TypeItem>();

    private Item.TypeItem typeCreatedItem;

    private void Awake() 
    {
        Item item = _createdItem.GetComponent<Item>();
        typeCreatedItem = item.type;
        StartCoroutine(HandleFactory());
    }

    private IEnumerator HandleFactory() 
    {
        while (true) 
        {
            if (_outputItems.IsCanAddItem(_createdItem)) 
            {
                if (RemoveItemsReceipt()) 
                {
                    isWork = true;
                    yield return new WaitForSeconds(_timeCreatedItem);
                    CreteItem();
                }
                else 
                {
                    if (isWork) 
                    {
                        onStop?.Invoke(ReasonStop.NoResource, typeCreatedItem);
                        isWork = false;
                    }
                    yield return new WaitForSeconds(0.1F);
                }

            } else 
            {
                if (isWork) 
                {
                    onStop?.Invoke(ReasonStop.FullStorage, typeCreatedItem);
                    isWork = false;
                }
                yield return new WaitForSeconds(0.1F);
            }
        }
    }

    protected virtual bool RemoveItemsReceipt() 
    {
        if (_inputItems != null && _receipt != null) 
        {
            if (_inputItems.countItems < _receipt.Length) return false;
            var removedItemsFromInventory = new List<GameObject>();

            foreach (Item.TypeItem typeItem in _receipt) 
            {
                GameObject objItem = _inputItems.GetLastItem(typeItem);
                if (objItem == null) 
                {
                    foreach (GameObject remObjItem in removedItemsFromInventory) 
                    {
                        _inputItems.AddItem(remObjItem);
                    }
                    return false;
                }

                _inputItems.RemoveItem(objItem);
                removedItemsFromInventory.Add(objItem);
            }
            Transform transform = gameObject.transform;
            foreach (GameObject objItem in removedItemsFromInventory) 
            {
                objItem.transform.SetParent(transform);
                MoverGameObject move = objItem.GetComponent<MoverGameObject>();
                
                move.MoveTo(_removedPosItem, Vector3.zero);
                move.onEndMove.AddListener(delegate () { Destroy(objItem);  });
            }
        }
        return true;
    }

    protected virtual void CreteItem() 
    {
        Transform transform = GetComponent<Transform>();
        GameObject objItem = Instantiate(_createdItem);

        Transform transformItem = objItem.transform;
        transformItem.position = transform.TransformPoint(_createdItemPos);
        transformItem.rotation = transform.rotation * Quaternion.Euler(_createdItemRotation);

        if (!_outputItems.AddItem(objItem)) 
            Debug.LogError("Error add item to storage");
    }

}
