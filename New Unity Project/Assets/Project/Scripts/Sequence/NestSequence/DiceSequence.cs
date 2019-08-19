﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DiceSequence : NestSequence<DiceSequence.State>
{
    public enum State
    {
        Start,
        RollWait,
        Rolling,
        Show,
        Cancel,
        End


    }
    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnRollStart = new UnityEvent();
    public UnityEvent OnThrow = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public UnityEvent OnUseTurnAction = new UnityEvent();//ターン消費が確定
    public DiceSequence(System.Action<DiceSequence> init)
    {
        init?.Invoke(this);
    }
    IEnumerator Start()
    {
        Debug.Log("[DiceSeq] Start");
        OnRollStart?.Invoke();
        _statemachine.Next(State.RollWait);
        yield return null;
    }
    IEnumerator RollWait()
    {
        Debug.Log("[DiceSeq] RollWait");
        yield return InputManager.Instance.WaitForButton(PlayerManager.Instance.CurrentPlayerModel,"Roll");
        OnThrow?.Invoke();
        _statemachine.Next(State.Rolling);
        yield return null;
    }
    IEnumerator Rolling()
    {
        Debug.Log("[DiceSeq] Rolling");
        yield return new WaitForSecondsForStatemachine(1f);
        _statemachine.Next(State.Show);
        yield return null;
    }
    IEnumerator Show()
    {
        Debug.Log("[DiceSeq] Show");
        OnUseTurnAction?.Invoke();
        _statemachine.Next(State.End);
        yield return null;
    }

    protected override void InitStatemachine()
    {
        _statemachine.Init(this);
    }
}
