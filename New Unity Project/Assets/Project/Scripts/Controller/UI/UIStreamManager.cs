using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using Toast;

public class PrefabManager : SingletonMonoBehaviour<PrefabManager>
{


    private Dictionary<PrefabModel.Path, GameObject> _prefabPool = new Dictionary<PrefabModel.Path, GameObject>();
    public Canvas Canvas { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        Canvas = Instantiate(LoadAndPoolPrefab(PrefabModel.Path.UICanvas))?.GetComponent<Canvas>();
    }


    #region プレハブ読み込み
    public GameObject InstantiateOn(PrefabModel.Path prefab)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        if (go.GetComponent<RectTransform>())
        {
            go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
        {
            transform.position = Vector2.zero;
        }
        
        return go;
    }
    public GameObject InstantiateOn(PrefabModel.Path prefab, Transform parent)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        go.transform.parent = parent ?? Canvas.transform;
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return go;
    }
    private GameObject InstantiateOn(PrefabModel.Path prefab, Transform parent, Vector2 pos)
    {
        var go = Instantiate(LoadAndPoolPrefab(prefab));
        go.transform.parent = parent;
        go.GetComponent<RectTransform>().anchoredPosition = pos;
        return go;
    }
    private GameObject LoadAndPoolPrefab(PrefabModel.Path prefab)
    {
        if (_prefabPool.ContainsKey(prefab))
        {
            return _prefabPool[prefab];
        }
        var go = LoadPrefab(prefab);
        _prefabPool.Add(prefab, go);
        return go;
    }
    private GameObject LoadPrefab(PrefabModel.Path prefab)
    {

        ResourcePath path = prefab.GetAttribute<ResourcePath>();
        if (path == null)
        {
            return null;
        }
        Debug.Log("[PrefabManager]LoadPrefab " + path);
        return Resources.Load<GameObject>(path.Path);
    }

    #endregion
    
}


public class UIStream : IDisposable
{
    private readonly List<IDisposable> _objects = new List<IDisposable>();

    public T Render<T,TM>(TM element) where T : UIViewBase<TM> where TM : UIModelBase
    {
        var ui = PrefabManager.Instance.InstantiateOn(element.PrefabPath, element.Parent).GetComponent<T>();
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