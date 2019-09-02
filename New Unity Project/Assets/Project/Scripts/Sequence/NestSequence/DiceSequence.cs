using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DiceSequence : NestSequence<DiceSequence.State>
{
    public enum State
    {
        Start,
        RollWait,
        Move,
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
    public GameSystemController Game;
    private int _diceNum = 0;
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
        //もう決めておく
        var dice = PlayerManager.Instance.CurrentPlayerModel.Dice.Roll();
        _diceNum = dice;
        var anim = PrefabManager.Instance.InstantiateOn(PrefabModel.Path.DiceAnim).GetComponent<DiceView>();
        yield return InputManager.Instance.WaitForButton(PlayerManager.Instance.CurrentPlayerModel,"Roll");
        anim.SetText(dice.ToString());
        yield return anim.WaitForAnimation();
        OnThrow?.Invoke();
        _statemachine.Next(State.Move);
        yield return null;
    }
    IEnumerator Move()
    {
        yield return Game.CalcMovable(PlayerManager.Instance.CurrentPlayerModel.Status.MapPos, _diceNum);

        var pos = Vector2Int.zero;
        var decide = false;
        while (!decide)
        {
            yield return InputManager.Instance.WaitForSelectMap(PlayerManager.Instance.CurrentPlayerModel,
                p => pos = p);
            Debug.Log($"{pos}");

            if (Game.CheckMovable(Game.GetMapIndex(pos)))
            {
                decide = true;
            }
            yield return null;
        }
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
