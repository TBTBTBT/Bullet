using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

}

public class DramaNextModel
{
    public string Name;//選択肢名
    public DramaType Type;
}
public class DramaFrameModel
{
    public Sprite[] BackSprite;//若いほうが後ろ
    public Sprite[] LeftChara;//
    public Sprite[] RightChara;
    public string Text;
}