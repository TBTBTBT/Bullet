using System;
using System.Collections;
using System.Collections.Generic;
using Toast;
using UnityEngine;

public class NetworkManagerDummy : SingletonMonoBehaviour<NetworkManagerDummy>
{

    public IEnumerator GetData(Action cb,float span = 0.5f)
    {
        var now = Time.time;
        while (Time.time - now < span)
        {
            yield return null;
        }
        cb?.Invoke();
    }
}
