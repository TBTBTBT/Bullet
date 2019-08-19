using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public enum PlayerType
{
    Local,
    Network,
    Com
}
public abstract class PlayerModelBase
{
    //public class IntEvent : UnityEvent<int> { }
    public string Id { get; set; } = "";
    //public IntEvent OnSelected = new IntEvent();
    //public IntEvent OnInputEvent = new IntEvent();
    public abstract PlayerModelBase Init(string id);
    public abstract void Update();

}
public class LocalPlayerModel : PlayerModelBase
{
    public override PlayerModelBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
public class NetworkPlayerModel : PlayerModelBase
{
    public override PlayerModelBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
public class ComPlayerModel : PlayerModelBase
{
    public override PlayerModelBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}