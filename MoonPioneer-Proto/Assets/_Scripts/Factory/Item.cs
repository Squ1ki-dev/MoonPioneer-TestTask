using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour 
{
    public enum TypeItem
    { 
        Blue, 
        Green, 
        Yellow 
    }

    [SerializeField] private TypeItem _type;
    public TypeItem type { get { return _type; } }
}
