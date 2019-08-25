 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
public class ButtonUI : UIViewBase<ButtonUIModel>
{
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _text;
    public IEnumerator WaitForClick()
    {
        var onClick = false;
        _button.onClick.AddListener(() => onClick = true);
        while (!onClick)
        {
            yield return null;
        }
        //Destroy(this.gameObject);
    }
    public override void Init(ButtonUIModel element)
    {
        base.Init(element);
        _button.onClick.AddListener(() => element.CallbackUp?.Invoke());
        _text.text = element.Label;
    }

    public void SetActive(bool flag)
    {
        gameObject.SetActive(flag);
    }
}
