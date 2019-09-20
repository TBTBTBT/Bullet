using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPrefs
{
    public Dictionary<string,int> Data = new Dictionary<string, int>();

    public int GetData(string key)
    {
        if (Data.ContainsKey(key))
        {
            return Data[key];
        }

        return 0;
    }

    public void SetData(string key,int data)
    {
        if (Data.ContainsKey(key))
        {
            Data[key] = data;
            return;
        }
        Data.Add(key,data);
    }
}
public abstract class PlayerEventBase 
{

    private IEnumerator _playing;
    public abstract IEnumerator Play( MstPlayerEventRecord record );

    public object Current { get; }
}
public abstract class AllEventBase
{
    public abstract IEnumerator Play();
}
public class Ev00_GameStart : AllEventBase
{
    public override IEnumerator Play()
    {
        throw new System.NotImplementedException();
    }
}
public class Pev00_Battle : PlayerEventBase
{
    public override IEnumerator Play(MstPlayerEventRecord record)
    {
        throw new System.NotImplementedException();
    }
}

public class Pev01_WeaponGet : PlayerEventBase
{
    public override IEnumerator Play(MstPlayerEventRecord record)
    {
        var progress = PlayerManager.Instance.CurrentPlayerModel.EventProgress.GetData("Pev01_WeaponGet");
        yield return DramaManager.Instance.Play(record.DramaType);
    }
}