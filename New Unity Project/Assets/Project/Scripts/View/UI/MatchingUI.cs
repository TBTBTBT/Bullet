﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class MatchingUI : UIViewBase<PulldownListUIModel>
{
    List<SelectListItemUi> _uis;
    List<PlayerType> valueRef { get; set; }

    public virtual void Init(PulldownListUIModel element)
    {
        base.Init(element);
        element.ChildUIModel.Parent = transform;
        
        foreach (var elementLabel in element.Labels)
        {
            element.ChildUIModel.Callback = num =>
            {
                
            }
            //Stream.Render<SelectListItemUi, PulldownItemUIModel>(element.ChildUIModel);
            
        }
    }
    public void Init(GameObject prefab, ref List<PlayerType> value, UnityAction<int> indexCallback)
    {
        var count = 0;
        _uis = new List<SelectListItemUi>();
        valueRef = value;
        foreach (PlayerType l in value)
        {
            var c = count;
            var item = Instantiate(prefab, transform).GetComponent<SelectListItemUi>();
            item.Init(l.ToString(), () => indexCallback?.Invoke(c));
            _uis.Add(item);
            count++;
        }
        
    }
    void Update()
    {
        UpdataView();
    }
    void UpdataView()
    {
        var count = 0;
        foreach (var value in valueRef)
        {
            if (_uis.Count > count)
            {
                _uis[count].SetText(value.ToString());
            }
            count++;
        }
    }
}
