using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class GameMenuSequence : NestSequence<GameMenuSequence.State>
{
    public enum State
    {
        Start,
        Select,
        End
    }
    public enum MenuType
    {
        Dice,
        Item,
        Magic,
        Data,
        WatchMap,
    }

    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnSelectMenu = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();
    IEnumerator Start()
    {
        Debug.Log("[MenuSequence] Start");
        OnStart?.Invoke();
        yield return InputManager.Instance.WaitForButton(PlayerManager.Instance.CurrentPlayerModel, $"{PlayerManager.Instance.CurrentPlayerIndex}のターン");
        _statemachine.Next(State.Select);
        yield return null;
    }


    IEnumerator Select()
    {
        Debug.Log("[MenuSequence] Select");
        var turnEnd = false;
        while (!turnEnd)
        {
            MenuType selected = MenuType.Dice;
            //入力まち
            yield return InputManager.Instance.WaitForSelect<MenuType>(
                PlayerManager.Instance.CurrentPlayerModel,
                select=>
                {
                    selected = select;
                });
            OnSelectMenu?.Invoke();
            switch (selected)
            {
                case MenuType.Dice:
                    yield return new DiceSequence(seq =>
                    {
                        seq.OnUseTurnAction.AddListener(() => turnEnd = true);
                    });
                    break;
                case MenuType.Item:
                    break;
                case MenuType.Magic:
                    break;
                case MenuType.Data:
                    break;
                case MenuType.WatchMap:
                    break;
            }
        }
        //_nestSequence?.Init();
        _statemachine.Next(State.End);
    }

    protected override void InitStatemachine()
    {
        _statemachine.Init(this);
    }
}
