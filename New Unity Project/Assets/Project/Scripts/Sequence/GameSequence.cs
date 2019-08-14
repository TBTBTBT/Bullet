using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Toast;
/// <summary>
/// 順番とか使用キャラは決めているていで
/// </summary>
public class GameSequence : MonoBehaviour
{
    public enum State
    {
        Init,
        PlayerWait,
        PlayerMove,
        PlayerAction,
        PlayerEvent,
        PlayerBattle,
        NextPlayer,
        
    }

    private Statemachine<State> _statemachine;
    // Start is called before the first frame update
    void Start()
    {
        _statemachine = new Statemachine<State>();
    }

    // Update is called once per frame
    void Update()
    {
        _statemachine.Update();
    }
}
