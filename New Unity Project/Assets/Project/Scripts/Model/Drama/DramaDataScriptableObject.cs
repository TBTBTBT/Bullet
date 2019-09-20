using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "DramaData", fileName = "Drama_00")]

public class DramaDataScriptableObject : ScriptableObject
{
    public DramaModel Model;
    //public DramaFrameModelSerialized[] Frame;
    //public DramaNextModelSerialized[] Next;
    //public 
    //
}
[Serializable]
public class DramaNextModelSerialized
{
    public string Name;//選択肢名
    public DramaType Type;
}
[Serializable]
public class DramaFrameModelSerialized
{
    public Sprite[] BackSprite;//若いほうが後ろ
    public Sprite[] LeftChara;//
    public Sprite[] RightChara;
    public string Text;
}