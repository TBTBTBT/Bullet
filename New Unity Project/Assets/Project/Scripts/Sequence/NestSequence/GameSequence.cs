using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
using System;
using UnityEngine.Events;
using static GameSequence;
using static PlayerInputEnum;
/// <summary>
/// 使用キャラは決めているていで
/// </summary>
public class GameSequence : NestSequence<GameSequence.State>
{
    public enum State
    {
        Start,
        Entry,
        GameStart,
        //ループ
        AllEvent,//ゲーム開始イベントなどターン開始時イベント
        PlayerAction,
        //Move,
        PlayerEvent,//バトルなど個人のイベント
        AllEvent2,//今週のリザルト画面などターン終了時イベント
        NextPlayer,
        //ループここまで
        EndEvent,
        End
    }

    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    public GameSequence(Action<GameSequence> init = null)
    {
        init?.Invoke(this);
    }
    protected override void InitStatemachine()
    {
        _statemachine.Init(this);
    }

    private MapController _mapController;
    //private INestSequence _nestSequence;

    IEnumerator Start()
    {
        Debug.Log("[GameSequence] Init");
        //_nestSequence = new MenuSequence();
        //_nestSequence.Init();
        _statemachine.Next(State.Entry);
        yield return null;
    }
    IEnumerator Entry()
    {
        Debug.Log("[GameSequence] Entry");
        //nestSequence.Init();
        yield return new EntrySequence();
        _statemachine.Next(State.GameStart);
        yield return null;
    }
    IEnumerator GameStart()
    {
        _statemachine.Next(State.AllEvent);
        yield return null;
    }
    IEnumerator AllEvent()
    {
        yield return null;
        _statemachine.Next(State.PlayerAction);
    }
    IEnumerator PlayerAction()
    {
        yield return new GameMenuSequence();
        _statemachine.Next(State.PlayerEvent);
        yield return null;

    }
    IEnumerator PlayerEvent()
    {
        yield return new GameMenuSequence();
        _statemachine.Next(State.PlayerEvent);
        yield return null;

    }

}
