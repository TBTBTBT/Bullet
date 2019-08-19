using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLocal : InputBase
{
    public override IEnumerator WaitForButton(string text, Vector2 pos = default, Action cb = null)
    {
        pos = pos == null ? Vector2.zero : pos;
        yield return UIViewManager.Instance.WaitForButton(text,pos);
        cb?.Invoke();
    }
    public override IEnumerator WaitForSelect<T>(Action<T> cb)
    {
        foreach (var res in UIViewManager.Instance.WaitForSelectUIVertical<T>())
        {
            if (res != null)
            {
                cb((T)res);
                break;
            }
            yield return null;
        }

    }

}
