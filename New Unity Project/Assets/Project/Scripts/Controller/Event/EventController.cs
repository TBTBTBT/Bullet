using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController
{
    private MstPlayerEventRecord _data;
    public void DecideEvent()
    {
        //var dummy = new PlayerEventModel();
        //todo マスターから取得
        _data = Lottery();

    }
    public IEnumerator Play()
    {
        Debug.Log("[ PlayerEvent ] Play");
        var type = _data.EventType.GetAttribute<EventType>().Type;
        var ev =  (PlayerEventBase)Activator.CreateInstance(type);
        yield return ev.Play(_data);
        yield return null;
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
    public MstPlayerEventRecord Lottery()
    {

        var records = MasterdataManager.Records<MstPlayerEventRecord>();
        var selected = MasterdataManager.Get<MstPlayerEventRecord>(0);
        Debug.Log("[ PlayerEvent ] Event Id : " + selected.Id);
        return selected;
    }
}
