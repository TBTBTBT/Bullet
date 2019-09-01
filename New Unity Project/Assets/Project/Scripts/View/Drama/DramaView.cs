using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
public class DramaView : MonoBehaviour
{
    [Serializable] 
    class ImageSet
    {
        public Animator Anim;
        public Image Image;
        public TMP_Text Name;

    }
    [SerializeField] private TMP_Text _text;
    [SerializeField] private ImageSet[] _backGrownds;
    [SerializeField] private ImageSet[] _left;
    [SerializeField] private ImageSet[] _right;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetBG(DramaSpriteModel sprite,int index)
    {
        if(_backGrownds.Length <= index)
        {
            
            return;
        }
        _backGrownds[index].Image.sprite = sprite.Sprite;
        _backGrownds[index].Name.text = sprite.Name;
        _backGrownds[index].Anim.Play(sprite.AnimState.ToString(),0);
    }
    public void SetLeft(DramaSpriteModel sprite, int index)
    {
        if (_left.Length <= index)
        {

            return;
        }
        _left[index].Image.sprite = sprite.Sprite;
        _left[index].Name.text = sprite.Name;
        _left[index].Anim.Play(sprite.AnimState.ToString(), 0);
    }
    public void SetRight(DramaSpriteModel sprite, int index)
    {
        if (_right.Length <= index)
        {

            return;
        }
        _right[index].Image.sprite = sprite.Sprite;
        _right[index].Name.text = sprite.Name;
        _right[index].Anim.Play(sprite.AnimState.ToString(), 0);
    }
    public void SetText(string text)
    {
        _text.text = text;
    }
}
