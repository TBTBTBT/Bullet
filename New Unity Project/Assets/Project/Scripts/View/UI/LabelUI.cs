using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelUI : UIViewBase<LabelUIModel>
{
    [SerializeField] private TMP_Text _text;
    public override void Init(LabelUIModel element)
    {
        base.Init(element);
        _text.text = element.Text;
    }

    public void ChangeLabel(string l)
    {
        _text.text = l;

    }
}
