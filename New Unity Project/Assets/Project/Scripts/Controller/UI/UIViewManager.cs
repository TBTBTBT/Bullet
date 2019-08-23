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
        go.transform.parent = parent ?? Canvas.transform;
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


public class UIStream : IDisposable
{
    private readonly List<IDisposable> _objects = new List<IDisposable>();

    public T Render<T,TM>(TM element) where T : UIViewBase<TM> where TM : UIModelBase
    {
        var ui = UIViewManager.Instance.InstantiateOn(element.PrefabPath, element.Parent).GetComponent<T>();
        ui.Stream = this;
        ui.Init(element);
        _objects.Add(ui);
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