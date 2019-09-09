using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum UIState
{
    Loading,
    Stable
}
public interface IUIState
{
    UIState State { get; set; }
    void StartLoading();
    void OnLoaded();
}
