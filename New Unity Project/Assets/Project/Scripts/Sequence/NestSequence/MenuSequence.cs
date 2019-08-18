using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MenuSequence : NestSequence<MenuSequence.State>
{
    public class MenuEvent : UnityEvent<MenuType> { }
    public enum State
    {
        Start,
        Select,
        End
    }
    public enum MenuType
    {
        Game,
        Title,

    }

    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public MenuSequence(Action<MenuSequence> init = null) => init?.Invoke(this);
    public UnityEvent OnStart = new UnityEvent();
    public MenuEvent OnSelectMenu = new MenuEvent();
    public UnityEvent OnEnd = new UnityEvent();
    IEnumerator Start()
    {
        Debug.Log("[MenuSequence] Start");
        OnStart?.Invoke();
        _statemachine.Next(State.Select);
        yield return null;
    }

    IEnumerator Select()
    {
        var select = MenuType.Title;
        foreach (var res in UIViewManager.Instance.WaitForSelectUIVertical<MenuType>()) 
        {
            if(res != null)
            {
                select = (MenuType)res;
                break;
            }
            yield return null;
        }
        OnSelectMenu?.Invoke(select);
        _statemachine.Next(State.End);
        yield return null;
    }
    protected override void InitStatemachine()
    {
        _statemachine.Init(this);
    }
}
