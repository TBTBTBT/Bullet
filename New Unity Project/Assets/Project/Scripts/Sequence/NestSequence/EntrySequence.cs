using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Toast;
public class EntrySequence : NestSequence<EntrySequence.State>
{
    public enum State
    {
        Start,
        TypeSelect,
        Host,
        Client,
        End
    }
    public enum Type
    {
        Host,
        Client
    }
    protected override State StartState() => State.Start;
    protected override State EndState() => State.End;
    public UnityEvent OnStart = new UnityEvent();
    public UnityEvent OnEnd = new UnityEvent();

    protected override void InitStatemachine() => _statemachine.Init(this);
    public EntrySequence(System.Action<EntrySequence> init = null)
    {
        init?.Invoke(this);
    }
    IEnumerator Start()
    {
        _statemachine.Next(State.TypeSelect);
        yield return null;
    }
    IEnumerator TypeSelect()
    {
        var type = Type.Host;
        foreach (var t in UIViewManager.Instance.WaitForSelectUIVertical<Type>()) {
            yield return null;
            if (t == null)
            {
                
                continue;
            }
            type = (Type)t;
        }
        switch (type)
        {
            case Type.Host:
                _statemachine.Next(State.Host);
                break;
            case Type.Client:
                _statemachine.Next(State.Client);
                break;
        }
        yield return null;

    }
    IEnumerator Host()
    {
     
        EntryManager.Instance.Reset();
        yield return EntryManager.Instance.WaitForEntry();
        EntryManager.Instance.Entry.ForEach(_ => PlayerManager.Instance.AddPlayer(_.Id,_.type));
        EntryManager.Instance.Reset();
        _statemachine.Next(State.End);
    }
    IEnumerator Client()
    {
        yield return null;
    }
    

}
public class EntryManager : Singleton<EntryManager>
{
    public enum LocalEntry
    {
        None,
        Local,
        Com
    }
    public List<(string Id, PlayerType type)> Entry = new List<(string Id, PlayerType type)>();
    public List<PlayerType> _entryCapacity = new List<PlayerType>();
    public int EntryNum => _entryCapacity.Count;
    //private UIObject _selectUiBuffer = null;
    public IEnumerator WaitForEntry()
    {
        var maxNum = 4;
        for(int i = 0; i < maxNum; i++)
        {
            _entryCapacity.Add(PlayerType.Local);
        }
        Debug.Log("[EntryManager]WaitForEntry");
        using (var ui = new UIStream())
        {
            var button = ui.Render<ButtonUI, ButtonUIModel>(new ButtonUIModel()
            {
                PrefabPath = PrefabModel.UI.Button,
                Label = "Ok"
            });
            
            var pulldown = ui.Render<MatchingUI, PulldownListUIModel>(new PulldownListUIModel()
            {
                PrefabPath = PrefabModel.UI.MatchingItem,
                ChildUIModel = new SelectItemUIModel()
                {

                }
            });
            button.SetActive(false);
            yield return button.WaitForClick();

        }
            //var ui = UIViewManager.Instance.ShowMatchingUI(ref _entryCapacity,index =>
        //{
        //    OnStartSelectEntryType(_entryCapacity[index],index);
        //});
        //UIObject button = null;
        //var next = false;
        //while (!next)
        //{
        //    yield return null;
        //    if (EntryNum < maxNum)
        //    {
        //        if(button != null)
        //        {
        //            button.Delete();
        //            button = null;
        //        }
        //        continue;
        //    }
        //    if (button == null)
        //    {
        //        button = UIViewManager.Instance.ShowButtonUI("OK", new Vector2(0, 150), () => next = true);
        //    }
        //}
        //if (button != null)
        //{
        //    button.Delete();
        //    button = null;
        //}
        

        //ui.Delete();
        //_selectUiBuffer?.Delete();
        //_selectUiBuffer = null;
        ConvertEntry();
        yield return WaitNetworkPlayer();
        Debug.Log("[EntryManager]EntryFinish");
    }
    public void Reset()
    {
        Debug.Log("[EntryManager]Reset");
        Entry.Clear();
        _entryCapacity.Clear();
        //_selectUiBuffer?.Delete();
        //_selectUiBuffer = null;
    }
    IEnumerator WaitNetworkPlayer()
    {
        
        if (Entry.Any(_=>_.type == PlayerType.Network)) {
            CreateWSServer();
            while (true)
            {
                Debug.Log("[EntryManager]WaitForAccess");
                yield return null;
            }
        }
    }
    void CreateWSServer()
    {
        var go = new GameObject("Server");
        go.AddComponent<WSServer>();
    }
    void OnStartSelectEntryType(PlayerType type,int index)
    {
        //todo いずれプルダウンに
        _selectUiBuffer = UIViewManager.Instance.ShowSelectUIVertical<PlayerType>(e=>OnChangeEntryType(e, index), new Vector2(100, 0));
    }
    /// <summary>
    /// ローカル操作で追加
    /// </summary>
    /// <param name=""></param>
    void OnChangeEntryType(PlayerType e,int index)
    {
        _entryCapacity[index] = e;
        _selectUiBuffer?.Delete();
        _selectUiBuffer = null;

    }
    void ConvertEntry()
    {
        var count = 0;
        foreach(var e in _entryCapacity)
        {
            var c = count;
            switch (e)
            {
                case PlayerType.Local:
                    Entry.Add(("Local_" + c, PlayerType.Local));
                    break;
                case PlayerType.Network:
                    Entry.Add(("", PlayerType.Network));
                    break;
                case PlayerType.Com:
                    Entry.Add(("Com_" + c, PlayerType.Com));
                    break;
            }
            count++;
        }
    }

}