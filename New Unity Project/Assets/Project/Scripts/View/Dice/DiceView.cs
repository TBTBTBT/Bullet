using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DiceView : MonoBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Animator _anim;
    private string _stateWait = "wait";
    private string _stateRoll = "roll";
    private string _stateShow = "show";
    public IEnumerator WaitForAnimation()
    {
        _anim.Play(_stateRoll);
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        if (_anim.GetCurrentAnimatorStateInfo(0).IsName(_stateShow))
        {
            Destroy(this.gameObject);
            yield break;
        }
        _anim.Play(_stateShow);
        yield return null;
        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }
        Destroy(this.gameObject);
    }
    public void SetText(string t)
    {
        _text.text = t;
    }
}
