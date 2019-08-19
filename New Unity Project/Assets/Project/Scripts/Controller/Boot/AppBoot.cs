using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class AppBoot : MonoBehaviour
{
    GameObject _singletons;
    GameObject _scripts;
    private void Awake()
    {

        //CreateSingleton<MasterdataManager>();
        //MasterdataManager.Instance.InitMasterdata();
        CreateSingleton<InputManager>();
        CreateSingleton<WholeSequence>();
        CreateSingleton<UIViewManager>();
        CreateScript<DebugManager>();
    }
    private void CreateSingleton<T>() where T : SingletonMonoBehaviour<T>
    {
        if(_singletons == null)
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
