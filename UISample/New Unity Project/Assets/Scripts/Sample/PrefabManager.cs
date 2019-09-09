using System.Collections;
using System.Collections.Generic;
using Toast;
using UnityEngine;

public class PrefabModel
{
    private const string RootPath = "Prefab/";
    public enum Path
    {
        [ResourcePath(RootPath + "Canvas")]
        UICanvas,
        [ResourcePath(RootPath + "Menu")]
        Menu,
    }
}
public class PrefabManager : SingletonMonoBehaviour<PrefabManager>
{

    
    private readonly Dictionary<PrefabModel.Path, GameObject> _prefabPool = new Dictionary<PrefabModel.Path, GameObject>();

    protected override void Awake()
    {
        base.Awake();
        
    }


    #region プレハブ読み込み
    public static GameObject InstantiateOn(PrefabModel.Path prefab)
    {
        var go = Instantiate(Instance.LoadAndPoolPrefab(prefab));
        if (go.GetComponent<RectTransform>())
        {
            go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
        {
            Instance.transform.position = Vector2.zero;
        }

        return go;
    }
    public static GameObject InstantiateOn(PrefabModel.Path prefab, Transform parent)
    {
        var go = Instantiate(Instance.LoadAndPoolPrefab(prefab));
        go.transform.parent = parent;
        go.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        return go;
    }
    private static GameObject InstantiateOn(PrefabModel.Path prefab, Transform parent, Vector2 pos)
    {
        var go = Instantiate(Instance.LoadAndPoolPrefab(prefab));
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
