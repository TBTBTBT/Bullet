using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ButtonUI : MonoBehaviour,IUIState
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Image _outline;
    [SerializeField] Image _body;

    public virtual UIState State { get; set; } = UIState.Loading;

    public void OnLoaded()
    {

    }

    public void SetColor(Color outline,Color body)
    {
        _text.color = outline;
        _outline.color = outline;
        _body.color = body;
    }

    public void StartLoading()
    {

    }
}
