using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LabelUI : UIViewBase
{
    [SerializeField] private TMP_Text _text;
    public void Init(string text)
    {
        _text.text = text;
    }
}
