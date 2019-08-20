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

}
