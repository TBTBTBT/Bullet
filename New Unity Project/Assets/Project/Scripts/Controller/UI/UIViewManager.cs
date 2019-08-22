using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Toast;
public class PrefabModel {
    private const string RootPath = "Prefab/";
    public enum UI
    {
        [ResourcePath(RootPath + "ui_canvas")]
        UICanvas,
        [ResourcePath(RootPath + "ui_list_vert")]
        VerticalSelectList,
        [ResourcePath(RootPath + "ui_list_item_vert")]
        VerticalSelectItem,
        [ResourcePath(RootPath + "ui_matching")]
        MatchingList,
        [ResourcePath(RootPath + "ui_matching_item")]
        MatchingItem,
        [ResourcePath(RootPath + "ui_button_normal")]
        Button,

    }
}
public class UIViewManager : SingletonMonoBehaviour<UIViewManager>
{
    
 
    private Dictionary<PrefabModel.UI, GameObject> _prefabPool = new Dictionary<PrefabModel.UI, GameObject>();
    public Canvas Canvas { get; private set;}
    private void Awake()
    {
        Canvas = Instantiate(LoadAndPoolPrefab(PrefabModel.UI.UICanvas))?.GetComponent<Canvas>();
    }

    //#region 選択メニュー(縦)
    public IEnumerable<T?> WaitForSelectUIVertical<T>() where T:struct
    {
        var listUi = InstantiateOn(PrefabModel.UI.VerticalSelectList,Canvas.transform);
        listUi.transform.parent = Canvas.transform;
        T? ret = null;
        foreach(T l in Enum.GetValues(typeof(T)))
        {
            var item = InstantiateOn(PrefabModel.UI.VerticalSelectItem, listUi.transform );
            item.GetComponent<SelectListItemUi>()?.Init(l.ToString(), () => ret = l);
        }
        while (ret == null)
        {
            yield return null;
        }
        Destroy(listUi);
        yield return ret;
    }
    //public UIObject ShowSelectUIVertical<T>(UnityAction<T> selectCallback) where T : struct
    //{
    //    return ShowSelectUIVertical<T>(selectCallback, Vector2.zero);
    //}
    //public UIObject ShowSelectUIVertical<T>(UnityAction<T> selectCallback,Vector2 pos) where T : struct
    //{
    //    var listUi = InstantiateOn(Prefabs.VerticalSelectList, Canvas.transform, pos);
    //    listUi.transform.parent = Canvas.transform;
    //    foreach (T l in Enum.GetValues(typeof(T)))
    //    {
    //        var item = InstantiateOn(Prefabs.VerticalSelectItem, listUi.transform, pos);
    //        item.GetComponent<SelectListItemUi>()?.Init(l.ToString(), () => selectCallback?.Invoke(l));
    //    }
    //    return new UIObject() { GameObject = listUi };
    //}
    //#endregion
    //#region マッチング
    //public UIObject ShowMatchingUI(ref List<PlayerType> value,UnityAction<int> indexCallback )// where T : struct
    //{
    //    var listUi = InstantiateOn(Prefabs.MatchingList, Canvas.transform);
    //    listUi.transform.parent = Canvas.transform;
    //    listUi.GetComponent<MatchingUI>()?.Init(LoadAndPoolPrefab(Prefabs.MatchingItem),ref value, indexCallback);

    //    return new UIObject() { GameObject = listUi };
    //}
    //#endregion
    //#region ボタン
    //public IEnumerator WaitForButton(string text,Vector2 pos)
    //{
    //    var listUi = InstantiateOn(Prefabs.Button, Canvas.transform, pos);
    //    listUi.transform.parent = Canvas.transform;
    //    yield return listUi.GetComponent<ButtonUI>().WaitForClick(text);

    //    Destroy(listUi);
    //}
    //public UIObject ShowButtonUI(string text , Vector2 pos , UnityAction callback)
    //{
    //    var listUi = InstantiateOn(Prefabs.Button, Canvas.transform, pos);
    //    listUi.transform.parent = Canvas.transform;
    //    listUi.GetComponent<ButtonUI>().Init(text, callback);
    //    return new UIObject() { GameObject = listUi };
    //}
    //#endregion

    #region プレハブ読み込み

    public GameObject InstantiateOn(PrefabModel.UI prefab, Transform parent)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        go.transform.parent = parent;
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return go;
    }
    private GameObject InstantiateOn(PrefabModel.UI prefab, Transform parent, Vector2 pos)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        go.transform.parent = parent;
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        return go;
    }
    private GameObject LoadAndPoolPrefab(PrefabModel.UI prefab)
    {
        if (_prefabPool.ContainsKey(prefab))
        {
            return _prefabPool[prefab];
        }
        var go = LoadPrefab(prefab);
        _prefabPool.Add(prefab, go);
        return go;
    }
    private GameObject LoadPrefab(PrefabModel.UI prefab)
    {

        ResourcePath path = prefab.GetAttribute<ResourcePath>();
        if (path == null)
        {
            return null;
        }
        Debug.Log("[UIViewManager]LoadPrefab");
        return Resources.Load<GameObject>(path.Path);
    }

    #endregion
    
}

public static class UI
{
    public static IEnumerator Render(params UIModelBase[] elements)
    {
        var end = false;

        using (var ui = new UIStream())
        {
            foreach (var uiModelBase in elements)
            {
                ui.AutoSwitchElement(uiModelBase,()=>end = true);
            }

            while (!end)
            {

                yield return null;
            }
        }

    }

    static void AutoSwitchElement(this UIStream stream,UIModelBase element,Action triggerAct)
    {
        var t = element.GetType().GetGenericArguments();
        switch (element)
        {
            case LabelUIModel e:
                stream.Render(e);
                break;
            case ButtonUIModel e:
                if (e.EndTrigger)
                {
                    var action = e.CallbackUp;
                    e.CallbackUp = () =>
                    {
                        action?.Invoke();
                        triggerAct();
                    };
                }
                stream.Render(e);
                break;
            case SelectUIModel e:
                if (e.EndTrigger)
                {
                    var action = e.Callback;
                    e.Callback = n =>
                    {
                        action?.Invoke(n);
                        triggerAct();
                    };
                }
                stream.Render(e);
                break;
            case SelectItemUIModel e:
                stream.Render(e);
                break;
                
        }
    }
}
public class UIStream : IDisposable
{
    private List<UIViewBase> _objects = new List<UIViewBase>();

    private UIViewBase AddUI(UIModelBase element)
    {
        var go = UIViewManager.Instance.InstantiateOn(element.PrefabPath, element.Parent);
        var component = go.GetComponent<UIViewBase>();
        if (component == null)
        {
            return null;
        }
        go.GetComponent<RectTransform>().anchoredPosition = element.Position;
        go.GetComponent<RectTransform>().sizeDelta = element.Size;
        go.GetComponent<RectTransform>().localScale = element.Scale;
        return component;
    }

    public ButtonUI Render(ButtonUIModel element)
    {
        var ui = AddUI(element) as ButtonUI;
        ui.Init(element.Label,()=>element.CallbackUp());
        return ui;
    }
    public LabelUI Render(LabelUIModel element)
    {
        var ui = AddUI(element) as LabelUI;
        ui.Init(element.Text);
        return ui;
    }
    public SelectListUi Render(SelectUIModel element)
    {
        
        var ui = AddUI(element) as SelectListUi;
        element.ChildUIModel.Parent = ui.transform;
        for (int i = 0; i < element.Labels.Length; i++)
        {
            var num = i;
            void Act() => element.Callback(num);
            element.ChildUIModel.Label = element.Labels[i];
            element.ChildUIModel.Callback = Act;
           var item = Render(element.ChildUIModel);
        }


        return ui;
    }

    public SelectListItemUi Render(SelectItemUIModel element)
    {
        var ui = AddUI(element) as SelectListItemUi;
        ui.Init(element.Label, () => element.Callback?.Invoke() );
        return ui;
    }
    public void Dispose()
    {
        _objects.RemoveAll(_ =>
        {
            _.Dispose();
            return true;
        });
        Debug.Log("[UIStream] Dispose");
    }
}