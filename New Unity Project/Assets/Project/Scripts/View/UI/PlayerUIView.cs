using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerUIView : UIViewBase<PlayerModel>
{
    [SerializeField] StatusUIView _status;
    public void SetView()
    {

    }


    public override void Init(PlayerModel element)
    {
        base.Init(element);
        GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);
        _status.Init(element.Status);
    }

}
