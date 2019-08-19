using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNetwork : InputBase
{
    public override IEnumerator WaitForButton(string text, Vector2 pos = default, Action cb = null)
    {
        throw new NotImplementedException();
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
