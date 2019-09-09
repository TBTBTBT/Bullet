using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteModel
{
    private const string RootPath = "Sprite/";
    public enum Path
    {
        [ResourcePath(RootPath + "")]
        Button,
        [ResourcePath(RootPath + "")]
        Menu,
    }
}

public class SpriteLoadStatus
{
    public enum State
    {
        Loading,
        Finish
    }

    public State Status = State.Loading;
}
public class SpriteManager : MonoBehaviour
{
    public SpriteLoadStatus LoadSpriteFromResource(SpriteModel.Path sprite,Action<Sprite> cb)
    {
        var status = new SpriteLoadStatus();
        StartCoroutine(LoadSpriteFromResourceCoroutine(sprite,status, cb));
        return status;
    }

    IEnumerator LoadSpriteFromResourceCoroutine(SpriteModel.Path sprite,SpriteLoadStatus status,Action<Sprite> cb)
    {
        ResourcePath path = sprite.GetAttribute<ResourcePath>();
        if (path == null)
        {
            yield break;
        }
        var req = Resources.LoadAsync<Sprite>(sprite.GetAttribute<ResourcePath>().Path);
        
        yield return req;
        cb.Invoke(req.asset as Sprite);
        status.Status = SpriteLoadStatus.State.Finish;
    }
}
