using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class UIModelBase
{
    public Vector2 Position = Vector2.zero;
    public Vector2 Size = Vector2.zero;
    public Vector2 Scale = Vector2.one;
    public UIViewManager.Prefabs PrefabPath;
    public Transform Parent;


}

public class LabelUIModel : UIModelBase
{
    public string Text;
}
public class SelectUIModel<T> : UIModelBase where T : struct
{
    public Action<T> Callback;
    public GridLayoutGroup.Axis Axis = GridLayoutGroup.Axis.Vertical;
    public Vector2 GridSize = Vector2.zero;
    public Vector2 GridPadding = Vector2.one;

}
public class Button : UIModelBase
{
    public Action CallbackDown;
    public Action CallbackUp;

}
