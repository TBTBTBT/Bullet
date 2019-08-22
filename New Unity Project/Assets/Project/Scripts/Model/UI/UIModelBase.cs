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
    public PrefabModel.UI PrefabPath;
    public Transform Parent;
    public bool EndTrigger;

}

public class LabelUIModel : UIModelBase
{
    public string Text;
}

public class SelectUIModel : UIModelBase
{
    public string[] Labels;
    public Action<int> Callback;
    public SelectItemUIModel ChildUIModel;
    public GridLayoutGroup.Axis Axis = GridLayoutGroup.Axis.Vertical;
    public Vector2 GridSize = Vector2.zero;
    public Vector2 GridPadding = Vector2.zero;
}
public class PulldownListUIModel : UIModelBase
{
    public string[] Labels;
    public Action<int> Callback;
    public SelectItemUIModel ChildUIModel;
    public GridLayoutGroup.Axis Axis = GridLayoutGroup.Axis.Vertical;
    public Vector2 GridSize = Vector2.zero;
    public Vector2 GridPadding = Vector2.zero;
}
public class PulldownUIModel : UIModelBase
{
    public string[] Labels;
    public Action<int> Callback;
    public SelectItemUIModel ChildUIModel;
    public GridLayoutGroup.Axis Axis = GridLayoutGroup.Axis.Vertical;
    public Vector2 GridSize = Vector2.zero;
    public Vector2 GridPadding = Vector2.zero;
}
public static class SelectUIModelExtensions
{
    public static SelectUIModel FromEnum<T>(this SelectUIModel model,Action<T> cb) where T : struct
    {
        model.Labels = Enum.GetNames(typeof(T));
        model.Callback = num => cb((T)Enum.GetValues(typeof(T)).GetValue(num));
        return model;
    }
}
public class SelectItemUIModel : UIModelBase
{
    public string Label;
    public Action Callback;
}
public class ButtonUIModel : UIModelBase
{
    public string Label;
    public Action CallbackDown;
    public Action CallbackUp;
}
