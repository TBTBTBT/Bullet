using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Toast.Rg
{

    public abstract class Singleton<T> where T : Singleton<T>, new()
    {
        public static T Instance { get; } = new T();
    }

    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    System.Type t = typeof(T);

                    _instance = (T)FindObjectOfType(t);
                    if (_instance == null)
                    {
                        Debug.LogError($" {t} が見つからない");
                    }
                }

                return _instance;
            }
        }
        virtual protected void Awake()
        {
            // 他のGameObjectにアタッチされているか調べる.
            // アタッチされている場合は破棄する.
            if (this != Instance)
            {
                Destroy(this);
                //Destroy(this.gameObject);
                Debug.Log(
                    typeof(T) + "はすでににアタッチされています。");
                return;
            }
            Debug.Log(typeof(T) + "が" + this.gameObject.name + "にアタッチされました");
            DontDestroyOnLoad(this.gameObject);
        }
    }

}
