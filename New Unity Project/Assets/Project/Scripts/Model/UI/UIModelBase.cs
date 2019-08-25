using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class UIModelBase
{
    public Vector2 Position = Vector2.zero;
    public Vector2? Size = null;
    public Vector2 Scale = Vector2.one;
    public PrefabModel.Path PrefabPath;
    public Transform Parent = null;

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
    public int Length;
    public Action<int> Callback;
    public PulldownItemUIModel ChildUIModel;
    public GridLayoutGroup.Axis Axis = GridLayoutGroup.Axis.Vertical;
    public Vector2 GridSize = Vector2.zero;
    public Vector2 GridPadding = Vector2.zero;
}
public class PulldownItemUIModel : UIModelBase
{
    public string[] Labels;
    public Action<int> Callback;
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



public static class UIModelExtensions
{
    public static SelectUIModel FromEnum<T>(this SelectUIModel model, Action<T> cb) where T : struct
    {
        model.Labels = Enum.GetNames(typeof(T));
        model.Callback = num => cb((T)Enum.GetValues(typeof(T)).GetValue(num));
        return model;
    }
    public static PulldownItemUIModel FromEnum<T>(this PulldownItemUIModel model, Action<T> cb = null) where T : struct
    {
        model.Labels = Enum.GetNames(typeof(T));
        //model.Callback = num => cb((T)Enum.GetValues(typeof(T)).GetValue(num));
        return model;
    }

}