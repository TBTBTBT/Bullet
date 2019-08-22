using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class ButtonUI : UIViewBase
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _text;
    public IEnumerator WaitForClick(string text)
    {
        var onClick = false;
        _text.text = text;
        _button.onClick.AddListener(() => onClick = true);
        while (!onClick)
        {
            yield return null;
        }
        //Destroy(this.gameObject);
    }
    public void Init(string text , UnityAction cb)
    {
        _button.onClick.AddListener(cb);
        _text.text = text;
    }
}
