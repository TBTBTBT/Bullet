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

public class DramaNextModel
{
    public string Name;//選択肢名
    public DramaType Type;
}
public class DramaSpriteModel
{
    public Sprite Sprite;
    public string Name;
    public DramaAnimationState AnimState;
}
public class DramaFrameModel
{
    public DramaSpriteModel[] BackSprite;//若いほうが後ろ
    public DramaSpriteModel[] LeftChara;//
    public DramaSpriteModel[] RightChara;
    public string Text;
}