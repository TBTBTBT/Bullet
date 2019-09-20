using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemModel 
{
    public ItemyType Type;
    public bool isCarry;
}
public enum ItemyType
{
    Gold,
    Equip,
    Usable,
}
