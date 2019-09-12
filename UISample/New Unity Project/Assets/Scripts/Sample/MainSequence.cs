using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Toast;
using UnityEditor;

public class MainSequence : SingletonMonoBehaviour<MainSequence>
{
    public enum State
    {
        Init,
        Choice,
        Do,
    }

    public enum Samples
    {
        Dialog1,
        Drama1,
        EndlessGrid1,
        EndlessGrid2,
        EndlessGrid3,
        Debug,

    }
    public Canvas Canvas { get; private set; }
    private Samples _selected;
    private Dictionary<Samples,(string name,Func<INestSequence> sequence)> _samples = new Dictionary<Samples, (string name, Func<INestSequence> sequence)>()
    {
        {Samples.Dialog1,("ダイアログシステム",() => new DialogSequence(null))},
        {Samples.EndlessGrid1,("デッキ編集",() => new DeckSequence(null))},
        {Samples.EndlessGrid2,("ミッション一覧",() => new DeckSequence(null))}
    };
    private readonly Statemachine<State> _stateMachine = new Statemachine<State>();

    protected void Awake()
    {
        _stateMachine.Init(this);
    }

    private void Update()
    {
        _stateMachine.Update();
    }
    IEnumerator Init()
    {
        Canvas = PrefabManager.InstantiateOn(PrefabModel.Path.UICanvas).GetComponent<Canvas>();
        DialogSingleton.SetPrefab(PrefabManager.GetPrefab(PrefabModel.Path.DialogCanvas).GetComponent<Dialog>());
        _stateMachine.Next(State.Choice);
        yield return null;
    }

    IEnumerator Choice()
    {
        foreach (Transform child in Canvas.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        var menu = PrefabManager.InstantiateOn(PrefabModel.Path.Menu, Canvas.transform).GetComponent<EndlessGrid>();
        var select = false;
        menu.InitOnStart = false;

        menu.ViewItemCount = 8;
        menu.ItemPrefab = PrefabManager.GetPrefab(PrefabModel.Path.Button);
        menu.OnUpdateElement.AddListener((go, index) =>
        {
            var key = _samples.Keys.ToArray()[index];
            if (_samples.ContainsKey(key))
            {
                go.GetComponent<AsyncButtonUI>().StartLoadingAndSetText(_samples[key].name);
                go.GetComponent<AsyncButtonUI>().Button.onClick.RemoveAllListeners();
                go.GetComponent<AsyncButtonUI>().Button.onClick.AddListener(() =>
                {
                    _selected = key;
                    select = true;
                });
            }
        });
        menu.AllItemCount = _samples.Count;
        menu.Initialize();
        while (!select)
        {
            yield return null;
        }
        Destroy(menu.gameObject);
        _stateMachine.Next(State.Do);
        yield return null;
    }

    IEnumerator Do()
    {
        yield return _samples[_selected].sequence();
        _stateMachine.Next(State.Choice);
        yield return null;
    }
}
