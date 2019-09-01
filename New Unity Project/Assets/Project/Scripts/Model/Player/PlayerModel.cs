using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum PlayerType
{
    Local,
    Network,
    Com
}
[Serializable]
public class PlayerModel
{
    //public class IntEvent : UnityEvent<int> { }
    public string Id = "";
    public PlayerType Type;
    public DiceModel Dice;
    public BelongModel Belong;
    public StatusModel Status;

    //public IntEvent OnSelected = new IntEvent();
    //public IntEvent OnInputEvent = new IntEvent();
    public PlayerModel Init(PlayerType type,string id)
    {
        Init(type, id, new int[] { 1, 2, 3, 4, 5, 6 });


        return this;
    }
    public PlayerModel Init(PlayerType type, string id,int[] d)
    {
        Id = id;
        SetDice(d);
        Belong = new BelongModel();
        Status = new StatusModel();
        Status.Pos = new Vector2Int(0, 0);
        return this;
    }
    public void SetDice(int[] d)
    {
        Dice = new DiceModel().Init(d);
    }


}
[Serializable]
public class DiceModel
{
    public const int DiceFaceNum = 6;
    public int[] Dice { get => _dice; }
    [SerializeField]
    private int[] _dice = new int[DiceFaceNum] { 1,2,3,4,5,6 };
    private bool ValidIndex(int index) => index < 0 || index >= DiceFaceNum;
    public int Roll()
    {
        int index = UnityEngine.Random.Range(0, 6);
        return Dice[index];
    }
    
    public void AddNumber(int index,int add)
    {
        if(ValidIndex(index))
        {
            _dice[index] += add;
        }
    }
    public DiceModel Init(int[] d)
    {
        for (int i = 0; i < DiceFaceNum; i++)
        {
            if (d.Length > i)
            {
                _dice[i] = d[i];
            }
            else
            {
                _dice[i] = 1;
            }
        }
        return this;
    }
}
/*
public class LocalPlayerModel : PlayerModelBase
{
    public override PlayerModelBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
public class NetworkPlayerModel : PlayerModelBase
{
    public override PlayerModelBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
public class ComPlayerModel : PlayerModelBase
{
    public override PlayerModelBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
*/