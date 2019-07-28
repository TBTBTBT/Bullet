using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
public class BattleSequence : MonoBehaviour
{
    public enum State
    {
        Init,

    }
    public int FrameCount { get; set; } = 0;
    public Statemachine<State> _statemachine;
    #region Sequence
    IEnumerator Init()
    {
        yield return null;
    }
    #endregion
    public void BattleStart()
    {

    }
    private void Awake()
    {
        _statemachine.Init(this);
    }
    private void Update()
    {
        
    }
}
