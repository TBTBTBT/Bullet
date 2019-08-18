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
public abstract class PlayerBase
{
    public class IntEvent : UnityEvent<int> { }
    public string Id { get; set; } = "";
    public IntEvent OnSelected = new IntEvent();
    public IntEvent OnInputEvent = new IntEvent();
    public abstract PlayerBase Init(string id);
    public abstract void Update();

}
public class LocalPlayer : PlayerBase
{
    public override PlayerBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
public class NetworkPlayer : PlayerBase
{
    public override PlayerBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}
public class ComPlayer : PlayerBase
{
    public override PlayerBase Init(string id)
    {
        Id = id;
        return this;
    }

    public override void Update()
    {
    }
}