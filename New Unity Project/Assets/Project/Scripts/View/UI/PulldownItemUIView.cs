using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PulldownItemUIView : UIViewBase<PulldownItemUIModel>
{
    //[SerializeField] TMP_Text _text;
    //[SerializeField] Button _button;
    [SerializeField] private Dropdown _dropdown;
    public override void Init(PulldownItemUIModel element)
    {
        base.Init(element);
        //_text.text = element.Label;
        _dropdown.options = new List<Dropdown.OptionData>();
        foreach (var elementLabel in element.Labels)
        {
            _dropdown.options.Add(new Dropdown.OptionData(elementLabel));
        }
        _dropdown.onValueChanged.AddListener(value=> element?.Callback?.Invoke(value));
        //_button.onClick.AddListener(() => element?.Callback?.Invoke());
    }


}
