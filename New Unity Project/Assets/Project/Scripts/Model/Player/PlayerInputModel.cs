using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Obsolete]
public static class PlayerInputEnum
{

    public enum Item
    {
        Use,
        Description,
        Delete,
    }
    public enum Select
    {
        SelectOne,
        SelectMap,
        NotSelect
    }
    public enum Magic
    {
        Use,
        Description,
        Delete,
    }
    public enum Move
    {
        CanGo,
        Move,
        WatchMap,
    }
    public enum Direction
    {
        None,
        Left,
        Up,
        Right,
        Down
    }
    public enum Battle
    {

    }
}
[Obsolete]
public static class ItemEnum
{
    public enum SelectType
    {
        SelectOne,
        SelectMap,
        NotSelect
    }
}

