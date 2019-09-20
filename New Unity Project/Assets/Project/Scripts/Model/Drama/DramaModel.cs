using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DramaAnimationState
{
    None,
    In,
    Out,
    Tilt,
}
[Serializable]
public class DramaModel
{
    //今のところ使わない
    public enum EndType
    {
        None,
        Select,
    }


    public DramaFrameModel[] Frame;
    public DramaNextModel[] Next;

    public ItemModel[] Rewards;
    //public StatusModel MoveStatus;

}
[Serializable]
public class DramaNextModel
{
    public string Name;//選択肢名
    public DramaType Type;
}
[Serializable]
public class DramaSpriteModel
{
    public Sprite Sprite;
    public string Name;
    public DramaAnimationState AnimState;
}
[Serializable]
public class DramaFrameModel
{
    public DramaSpriteModel[] BackSprite;//若いほうが後ろ
    public DramaSpriteModel[] LeftChara;//
    public DramaSpriteModel[] RightChara;
    public string Text;
}