using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class SelectListItemUi : UIViewBase
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Button _button;
    public void Init(string text, UnityAction action) {
        _text.text = text;
        _button.onClick.AddListener(action);
    }
    public void SetText(string text) {
        _text.text = text;
    }
}
