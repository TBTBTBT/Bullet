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
        [ResourcePath(RootPath + "Button")]
        Button,
        [ResourcePath(RootPath + "DialogCanvas")]
        DialogCanvas,
        [ResourcePath(RootPath + "DialogNormal")]
        DialogNormal,
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
        var p = Instance.LoadAndPoolPrefab(prefab);
        var go = Instantiate(p);
        go.transform.localScale = p.transform.localScale;
        go.transform.localPosition = p.transform.localPosition;
        var rect = go.GetComponent<RectTransform>();
        if (rect)
        {
            var prect = p.GetComponent<RectTransform>();
            rect.anchoredPosition = prect.anchoredPosition;
            rect.sizeDelta = prect.sizeDelta;
            rect.offsetMax = prect.offsetMax;
            rect.offsetMin = prect.offsetMin;
        }
        return go;
    }
    public static GameObject InstantiateOn(PrefabModel.Path prefab, Transform parent)
    {
        var p = Instance.LoadAndPoolPrefab(prefab);
        var go = Instantiate(p);
        go.transform.parent = parent;
        go.transform.localScale = p.transform.localScale;
        go.transform.localPosition = p.transform.localPosition;
        var rect = go.GetComponent<RectTransform>();
        if (rect)
        {
            var prect = p.GetComponent<RectTransform>();
            rect.anchoredPosition = prect.anchoredPosition;
            rect.sizeDelta = prect.sizeDelta;
            rect.offsetMax = prect.offsetMax;
            rect.offsetMin = prect.offsetMin;
        }

        return go;
    }

    public static GameObject GetPrefab(PrefabModel.Path prefab)
    {
        return Instance.LoadAndPoolPrefab(prefab);
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
