using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Toast;

public class WholeSequence : SingletonMonoBehaviour<WholeSequence>
{
    public enum State
    {
        Init,
        Advertise,
        Title,
        Menu,
        Game,
        Credit,
    }
    private Statemachine<State> _statemachine = new Statemachine<State>();

    protected override void Awake()
    {
        base.Awake();
        _statemachine.Init(this);
    }
    private void Update()
    {
        _statemachine.Update();
    }
    IEnumerator Init()
    {
        Debug.Log("[WholeSequence]Init");
        yield return null;
        _statemachine.Next(State.Advertise);
    }
    IEnumerator Advertise()
    {
        Debug.Log("[WholeSequence]Advertise");
        yield return null;
        _statemachine.Next(State.Title);
    }
    IEnumerator Title()
    {
        Debug.Log("[WholeSequence]Title");
        yield return new TitleSequence();
        _statemachine.Next(State.Menu);
    }
    IEnumerator Menu()
    {
        Debug.Log("[WholeSequence]Menu");
        var nextState = State.Title;
        yield return new MenuSequence(seq =>
        {
            seq.OnSelectMenu.AddListener(type => 
            {
                switch (type)
                {
                    case MenuSequence.MenuType.Game:
                        nextState = State.Game;
                        break;
                    case MenuSequence.MenuType.Title:
                        nextState = State.Title;
                        break;
                }
            });
        });
        _statemachine.Next(nextState);
    }
    IEnumerator Game()
    {
        yield return new GameSequence();
    }
}
