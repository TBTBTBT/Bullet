using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class MainSequence : MonoBehaviour
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
    private Dictionary<Samples,(string name,INestSequence sequence)> _samples = new Dictionary<Samples, (string name, INestSequence sequence)>()
    {
        {Samples.Dialog1,("ダイアログシステム",new DialogSequence(null))}
    };
    private readonly Statemachine<State> _stateMachine = new Statemachine<State>();

    protected void Awake()
    {
        _stateMachine.Init(this);
    }

    IEnumerator Init()
    {
        Canvas = PrefabManager.InstantiateOn(PrefabModel.Path.UICanvas).GetComponent<Canvas>();
        _stateMachine.Next(State.Choice);
        yield return null;
    }

    IEnumerator Choice()
    {
        yield return null;
    }

    IEnumerator Do()
    {
        yield return null;
    }
}
