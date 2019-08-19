using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputBase
{
    public abstract IEnumerator WaitForSelect<T>(Action<T> cb) where T : struct;
    public abstract IEnumerator WaitForButton(string text, Vector2 pos = default, Action cb = null);
}
