using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController
{
    private PlayerEventModel _data;
    public void DecideEvent()
    {
        //var dummy = new PlayerEventModel();
        //todo マスターから取得
        _data = Lottery();

    }
    public IEnumerator Play()
    {
        
        if (HasDrama())
        {
            yield return Drama(_data.Drama);
        }

        yield return null;
    }
    private bool HasDrama()
    {
        try
        {
            if (_data.Drama != DramaType.None)
            {
                return true;
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return false;
        }
        return false;
    }
    

    private IEnumerator Drama(DramaType type)
    {
        yield return DramaManager.Instance.Play(type);
    }
    //private IEnumerator Choice()
    //{

    //    yield return InputManager.Instance.WaitForSelect(PlayerManager.Instance.CurrentPlayerModel,new []{
    //        "Dummy",
    //        "Dummy",
    //    }, index =>
    //    {

    //    });
    //}
    //private IEnumerator Button()
    //{
    //    yield return null;
    //}
    //抽選
    public PlayerEventModel Lottery()
    {
        var cache = new PlayerEventModel();
        var records = MasterdataManager.Records<MstPlayerEventRecord>();

        return cache;
    }
}
