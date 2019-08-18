using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
public class TitleSequence : NestSequence<TitleSequence.State>
{
    public enum State
    {
        Start,
        Perform,
        WaitInput,
        End,

    }
    
    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public TitleSequence(Action<TitleSequence> init = null) => init?.Invoke(this);
    protected override void InitStatemachine() => _statemachine.Init(this);
    //ここまでテンプレ
    private MapManager _mapManager;
    //private INestSequence _nestSequence;

    IEnumerator Start()
    {
        Debug.Log("[TitleSequence] Start");
        //_nestSequence = new MenuSequence();
        //_nestSequence.Init();
        _statemachine.Next(State.Perform);
        yield return null;
    }
    IEnumerator Perform()
    {
        Debug.Log("[TitleSequence] Perform");
        //nestSequence.Init();
        _statemachine.Next(State.WaitInput);
        yield return null;
    }
    IEnumerator WaitInput()
    {
        yield return InputManager.Instance.WaitForButton("Start");
        _statemachine.Next(State.End);
    }


}
