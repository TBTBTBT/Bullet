using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Toast;
public class UIViewManager : SingletonMonoBehaviour<UIViewManager>
{
    enum Prefabs
    {
        [ResourcePath("Prefab/ui_canvas")]
        UICanvas,
        [ResourcePath("Prefab/ui_list_vert")]
        VerticalSelectList,
        [ResourcePath("Prefab/ui_list_item_vert")]
        VerticalSelectItem,
        [ResourcePath("Prefab/ui_matching")]
        MatchingList,
        [ResourcePath("Prefab/ui_matching_item")]
        MatchingItem,
        [ResourcePath("Prefab/ui_button_normal")]
        Button,

    }
    private Dictionary<Prefabs, GameObject> _prefabPool = new Dictionary<Prefabs, GameObject>();
    public Canvas Canvas { get; private set;}
    private void Awake()
    {
        Canvas = Instantiate(LoadAndPoolPrefab(Prefabs.UICanvas))?.GetComponent<Canvas>();
    }

    #region 選択メニュー(縦)
    public IEnumerable<T?> WaitForSelectUIVertical<T>() where T:struct
    {
        var listUi = InstantiateOn(Prefabs.VerticalSelectList,Canvas.transform);
        listUi.transform.parent = Canvas.transform;
        T? ret = null;
        foreach(T l in Enum.GetValues(typeof(T)))
        {
            var item = InstantiateOn(Prefabs.VerticalSelectItem, listUi.transform );
            item.GetComponent<SelectListItemUi>()?.Init(l.ToString(), () => ret = l);
        }
        while (ret == null)
        {
            yield return null;
        }
        Destroy(listUi);
        yield return ret;
    }
    public UIObject ShowSelectUIVertical<T>(UnityAction<T> selectCallback) where T : struct
    {
        return ShowSelectUIVertical<T>(selectCallback, Vector2.zero);
    }
    public UIObject ShowSelectUIVertical<T>(UnityAction<T> selectCallback,Vector2 pos) where T : struct
    {
        var listUi = InstantiateOn(Prefabs.VerticalSelectList, Canvas.transform, pos);
        listUi.transform.parent = Canvas.transform;
        foreach (T l in Enum.GetValues(typeof(T)))
        {
            var item = InstantiateOn(Prefabs.VerticalSelectItem, listUi.transform, pos);
            item.GetComponent<SelectListItemUi>()?.Init(l.ToString(), () => selectCallback?.Invoke(l));
        }
        return new UIObject() { GameObject = listUi };
    }
    #endregion
    #region マッチング
    public UIObject ShowMatchingUI(ref List<PlayerType> value,UnityAction<int> indexCallback )// where T : struct
    {
        var listUi = InstantiateOn(Prefabs.MatchingList, Canvas.transform);
        listUi.transform.parent = Canvas.transform;
        listUi.GetComponent<MatchingUI>()?.Init(LoadAndPoolPrefab(Prefabs.MatchingItem),ref value, indexCallback);

        return new UIObject() { GameObject = listUi };
    }
    #endregion
    #region ボタン
    public IEnumerator WaitForButton(string text,Vector2 pos)
    {
        var listUi = InstantiateOn(Prefabs.Button, Canvas.transform, pos);
        listUi.transform.parent = Canvas.transform;
        yield return listUi.GetComponent<ButtonUI>().WaitForClick(text);

        Destroy(listUi);
    }
    public UIObject ShowButtonUI(string text , Vector2 pos , UnityAction callback)
    {
        var listUi = InstantiateOn(Prefabs.Button, Canvas.transform, pos);
        listUi.transform.parent = Canvas.transform;
        listUi.GetComponent<ButtonUI>().Init(text, callback);
        return new UIObject() { GameObject = listUi };
    }
    #endregion
    private GameObject InstantiateOn(Prefabs prefab, Transform parent)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        go.transform.parent = parent;
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return go;
    }
    private GameObject InstantiateOn(Prefabs prefab,Transform parent,Vector2 pos)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        go.transform.parent = parent;
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        return go;
    }
    private GameObject LoadAndPoolPrefab(Prefabs prefab)
    {
        if (_prefabPool.ContainsKey(prefab)) 
        {
            return _prefabPool[prefab];
        }
        var go = LoadPrefab(prefab);
        _prefabPool.Add(prefab, go);
        return go;
    }
    private GameObject LoadPrefab(Prefabs prefab)
    {
        ResourcePath path = prefab.GetAttribute<ResourcePath>();
        if (path == null)
        {
            return null;
        }
        Debug.Log("[UIViewManager]LoadPrefab");
        return Resources.Load<GameObject>(path.Path);
    }
}
public class UIObject
{
    public GameObject GameObject { get; set; }
    public void Delete()
    {
        UnityEngine.Object.Destroy(GameObject);
    }
}