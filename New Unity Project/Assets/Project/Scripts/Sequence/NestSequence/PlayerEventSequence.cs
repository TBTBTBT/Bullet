using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventSequence : NestSequence<PlayerEventSequence.State>
{
    public enum State
    {
        Start,
        DecideEvent,
        Do,
        End,

    }

    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public PlayerEventSequence(Action<PlayerEventSequence> init = null) => init?.Invoke(this);
    protected override void InitStatemachine() => _statemachine.Init(this);
    //ここまでテンプレ
    private PlayerEventController _eventController;

    IEnumerator Start()
    {
        _statemachine.Next(State.DecideEvent);
        yield return null;
    }

    IEnumerator DecideEvent()
    {
        _eventController = new PlayerEventController();
        _eventController.DecideEvent();
        _statemachine.Next(State.Do);
        yield return null;
    }

    IEnumerator Do()
    {
        yield return _eventController.Play();
        _statemachine.Next(State.End);
        yield return null;
        
    }
}
