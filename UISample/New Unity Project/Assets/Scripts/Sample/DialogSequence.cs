using System;
using System.Collections;
using System.Collections.Generic;
using Toast;
using UnityEngine;
using UnityEngine.Events;

public class DialogSequence : SequenceBase<DialogSequence.State>
{
    public enum State
    {
        Start,
        Execute,
        End
    }


    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public DialogSequence(Action<DialogSequence> init = null) => init?.Invoke(this);
    protected override void InitStatemachine() => _statemachine.Init(this);
    // Start is called before the first frame update
    IEnumerator Start()
    {
        _statemachine.Next(State.Execute);
        yield return null;
    }

    IEnumerator Execute()
    {
        //ステートマシン上で動作するダイアログです。
        //
        yield return DialogSingleton.OpenModal(new Dialog.InputData()
        {
            
        });
        yield return null;
    }

}
