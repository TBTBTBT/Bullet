using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AsyncButtonUI : AsyncUIBehaviour
{
    [SerializeField] TMP_Text _text;
    [SerializeField] Image _outline;
    [SerializeField] Image _body;
    [SerializeField] private Animator _animator;
    public Button Button;
    public void SetColor(Color outline,Color body)
    {
        _text.color = outline;
        _outline.color = outline;
        _body.color = body;
    }

    protected override void Awake()
    {
        
        //StartLoading();
    }

    public void StartLoadingAndSetText(string text)
    {

        StartLoading();
        _text.text = text;
    }
    protected override IEnumerator Load()
    {
        _animator.Play("Loading");
        var time = Time.time;
        var wait = 0.1f;//Random.Range(0.5f, 2f);
        while (Time.time - time < wait)
        {
            yield return null;
        }
        
    }

    protected override void OnLoad()
    {
        _animator.Play("Show");
    }

    protected override void OnCanceled()
    {
        
    }
}
