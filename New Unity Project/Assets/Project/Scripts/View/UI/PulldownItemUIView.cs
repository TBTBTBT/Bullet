using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PulldownItemUIView : UIViewBase<PulldownItemUIModel>
{
    //[SerializeField] TMP_Text _text;
    //[SerializeField] Button _button;
    [SerializeField] private TMP_Dropdown _dropdown;
    public override void Init(PulldownItemUIModel element)
    {
        base.Init(element);
        //_text.text = element.Label;
        var options = new List<TMP_Dropdown.OptionData>();
        foreach (var elementLabel in element.Labels)
        {
            options.Add(new TMP_Dropdown.OptionData(elementLabel));
           
        }
        _dropdown.AddOptions(options);
        var cb = element?.Callback;
        _dropdown.onValueChanged.AddListener(value=> cb?.Invoke(value));
        
        //_button.onClick.AddListener(() => element?.Callback?.Invoke());
    }


}
