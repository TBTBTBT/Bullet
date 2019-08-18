using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
public static class ItemEnum
{
    public enum SelectType
    {
        SelectOne,
        SelectMap,
        NotSelect
    }
}

