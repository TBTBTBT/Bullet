using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class SelectListItemUi : UIViewBase<SelectItemUIModel>
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Button _button;

    public override void Init(SelectItemUIModel element)
    {
        base.Init(element);
        _text.text = element.Label;
        _button.onClick.AddListener(() =>element?.Callback?.Invoke());
    }

    public void SetText(string text) {
        _text.text = text;
    }



}
