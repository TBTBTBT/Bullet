using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Toast;
/// <summary>
/// 入力に関するデータ
/// </summary>
public class InputManager : SingletonMonoBehaviour<InputManager>
{
    private InputLocal _menuInput;
    private Dictionary<string, InputBase> _gameInput = new Dictionary<string, InputBase>();
    private void Awake()
    {
        _menuInput = new InputLocal();
        
    }
    void CheckAndAddPlayer(PlayerModel playerModel)
    {
        if (!_gameInput.ContainsKey(playerModel.Id))
        {
            switch (playerModel.Type)
            {
                case PlayerType.Local:
                    _gameInput.Add(playerModel.Id, new InputLocal());
                    break;
                case PlayerType.Network:
                    _gameInput.Add(playerModel.Id, new InputNetwork());
                    break;
                case PlayerType.Com:
                    _gameInput.Add(playerModel.Id, new InputLocal());
                    break;
            }

        }
    }
    public IEnumerator WaitForButton(string text,Vector2 pos = default,Action cb = null)
    {
        yield return _menuInput.WaitForButton(text,pos,cb);
    }
    public IEnumerator WaitForButton(PlayerModel playerModel, string text, Vector2 pos = default, Action cb = null)
    {
        CheckAndAddPlayer(playerModel);
        yield return _gameInput[playerModel.Id].WaitForButton(text,pos,cb);

    }
    
    public IEnumerator WaitForSelectMap(PlayerModel playerModel, Action<Vector2Int> cb)
    {
        CheckAndAddPlayer(playerModel);
        yield return _gameInput[playerModel.Id].WaitForSelectMap(cb);
    }
    public IEnumerator WaitForSelect<T>(PlayerModel playerModel, Action<T> cb) where T : struct
    {
        CheckAndAddPlayer(playerModel);
        yield return _gameInput[playerModel.Id].WaitForSelect<T>(cb);

    }
    public IEnumerator WaitForSelect(PlayerModel playerModel, string[] list,Action<int> cb)
    {
        CheckAndAddPlayer(playerModel);
        yield return _gameInput[playerModel.Id].WaitForSelect(list,cb);

    }
}

/*
class TouchEvent : UnityEvent<Vector2,InputDefine> { }
[Flags]
public enum InputDefine : uint
{
    A = 1<<0,
    B = 1<<1,
    Y = 1<<2,
    X = 1<<3,
    L = 1<<4,
    R = 1<<5,
    Start = 1<<6,
    Up = 1<<7,
    Down = 1<<8,
    Right = 1<<9,
    Left = 1<<10,
}
TouchEvent OnTouch = new TouchEvent();
List<InputBase> _inputs = new List<InputBase>();

public IEnumerator WaitForInput(InputDefine input,Rect screenRect)
{
    var isInput = false;
    OnTouch.AddListener((pos,type)=> 
    {
        if (screenRect.Contains(pos) && type.HasFlag(input))
        {
            isInput = true;
        }
    });
    while (!isInput)
    {
        yield return false;
    }

}
public IEnumerator WaitForInput(InputDefine input)
{
    var isInput = false;
    OnTouch.AddListener((pos, type) =>
    {
        if (type.HasFlag(input))
        {
            isInput = true;
        }
    });
    while (!isInput)
    {
        yield return false;
    }

}
public IEnumerable<T?> WaitForSelect<T>() where T : struct
{
    yield return null;
}
// Start is called before the first frame update
protected override void Awake()
{
    base.Awake();
    _inputs.Add(new InputLocal());
    //networkはInput情報は必要なく、結果だけ受け取る
    //_inputs.Add(new InputNetwork());
}
private void UpdateInputMouse()
{
    if (Input.GetMouseButtonDown(0))
    {
        Vector3 mousePosition = Input.mousePosition;

        Debug.Log("LeftClick:" + mousePosition);
        OnTouch.Invoke(mousePosition, InputDefine.Start | InputDefine.A);
    }
}
private void Update()
{
    UpdateInputMouse();
}
#if UNITY_EDITOR
#endif
#if !UNITY_EDITOR
#endif
*/
