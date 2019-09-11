using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum UIState
{
    Loadable,
    Loading,
    Stable
}
public abstract class AsyncUIBehaviour : UIBehaviour
{
    private UIState State { get; set; } = UIState.Loadable;
    protected bool _canOverride = true;
    private Coroutine _loading = null;
    public void StartLoading()
    {

        if (CheckCanLoad())
        {
            if (_loading != null)
            {
                StopCoroutine(_loading);
                State = UIState.Loadable;
                OnCanceled();
            }
            _loading = StartCoroutine(Loading());
        }
    }

    bool CheckCanLoad()
    {
        return State == UIState.Loadable || State == UIState.Stable || _canOverride;
    }
    IEnumerator Loading()
    {
        
        State = UIState.Loading;
        yield return Load();
        State = UIState.Stable;
        OnLoad();
    }

    protected abstract IEnumerator Load();

    protected abstract void OnLoad();
    protected abstract void OnCanceled();

}