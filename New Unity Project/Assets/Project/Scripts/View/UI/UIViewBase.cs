using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIViewBase<T> : MonoBehaviour,IDisposable where T : UIModelBase
{
    public UIStream Stream { protected get; set; }
    public virtual void Dispose()
    {
        Destroy(this.gameObject);
    }

    public virtual void Init(T element)
    {
        GetComponent<RectTransform>().anchoredPosition = element.Position;
        GetComponent<RectTransform>().sizeDelta = element.Size;
        GetComponent<RectTransform>().localScale = element.Scale;

    }
}
