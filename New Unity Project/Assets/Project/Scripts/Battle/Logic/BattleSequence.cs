using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class BattleSequence : MonoBehaviour
{
    public enum State
    {
        Init,
        WaitSignalReady,
        WaitSignalStart,
        Calcurate,
    }
    public int FrameCount { get; set; } = 0;
    public bool ReadyFlag { get; set; }
    public bool StartFlag { get; set; }
    public Statemachine<State> _statemachine;
    [SerializeField]
    private BattleManager _battleManager; 
    #region Sequence
    IEnumerator Init()
    {
        _battleManager.Init();
        yield return null;
    }
    IEnumerator WaitSignalReady()
    {
        _battleManager.AddCharacter();
        while (!ReadyFlag)
        {
            yield return null;
        }
        _statemachine.Next(State.WaitSignalStart);
        yield return null;
    }
    IEnumerator WaitSignalStart()
    {
        if (!StartFlag)
        {
            
        }
        _statemachine.Next(State.Calcurate);
        yield return null;
    }
    IEnumerator Calcurate()
    {
        while (true) {
            _battleManager.Simurate();
            yield return null;
        }
    }
    #endregion

    private void Awake()
    {
        _statemachine.Init(this);
    }
    private void Update()
    {
        _statemachine.Update();
    }
}
