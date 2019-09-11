using System.Collections;
using System.Collections.Generic;
using Toast;
using UnityEngine;

public class AppBoot : MonoBehaviour
{
    GameObject _singletons;
    GameObject _scripts;
    private void Awake()
    {
        //primary
        CreateSingleton<DialogSingleton>();
        CreateSingleton<PrefabManager>();
        //secondly
        CreateSingleton<MainSequence>();
    }
    private void CreateSingleton<T>() where T : SingletonMonoBehaviour<T>
    {
        if (_singletons == null)
        {
            _singletons = new GameObject();
            _singletons.isStatic = true;
        }
        _singletons.AddComponent<T>();

    }
    private void CreateScript<T>() where T : MonoBehaviour
    {
        if (_scripts == null)
        {
            _scripts = new GameObject();
            _scripts.isStatic = true;
        }
        _scripts.AddComponent<T>();
    }
}