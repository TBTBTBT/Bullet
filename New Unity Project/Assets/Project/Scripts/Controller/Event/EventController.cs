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
            yield return Drama();
        }

        yield return null;
    }
    private bool HasDrama()
    {
        //if (_data)
        {
            return true;
        }
        return false;
    }
    

    private IEnumerator Drama()
    {
        yield return null;
    }
    private IEnumerator Choice()
    {

        yield return InputManager.Instance.WaitForSelect(PlayerManager.Instance.CurrentPlayerModel,new []{
            "Dummy",
            "Dummy",
        }, index =>
        {

        });
    }
    private IEnumerator Button()
    {
        yield return null;
    }
    //抽選
    public PlayerEventModel Lottery()
    {
        var cache = new PlayerEventModel();
        var records = MasterdataManager.Records<MstPlayerEventRecord>();

        return cache;
    }
}
