using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Toast;
using Toast.Rg;
using UnityEngine.Events;
using UnityEngine;
public class DeckSequence : SequenceBase<DeckSequence.State>
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
    public DeckSequence(Action<DeckSequence> init = null) => init?.Invoke(this);
    protected override void InitStatemachine() => _statemachine.Init(this);
    
    public List<long> CardList = new List<long>();
    IEnumerator Start()
    {
        yield return Init();
        _statemachine.Next(State.Execute);
        yield return null;
    }

    IEnumerator Execute()
    {

        //_statemachine.Next(State.End);
        yield return null;
    }

    IEnumerator Init()
    {
        for (long i = 0; i < 200; i++)
        {
            CardList.Add(i);
        }
       
        var menu = PrefabManager.InstantiateOn(PrefabModel.Path.Menu, MainSequence.Instance.Canvas.transform).GetComponent<EndlessGrid>();
        var select = false;
        menu.InitOnStart = false;
        menu.ViewItemCount = 15;
        menu.ItemPrefab = PrefabManager.GetPrefab(PrefabModel.Path.Button);
        menu.OnUpdateElement.AddListener((go, index) =>
        {
            go.GetComponent<AsyncButtonUI>().StartLoadingAndSetText($"id:{CardList[index]}");
            go.GetComponent<AsyncButtonUI>().Button.onClick.RemoveAllListeners();
            go.GetComponent<AsyncButtonUI>().Button.onClick.AddListener(() =>
            {
                select = true;
            });
        });
        menu.AllItemCount = CardList.Count;
        menu.Initialize();
        yield return null;
    }
    
}
