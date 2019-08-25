using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
public class MatchingUI : UIViewBase<PulldownListUIModel>
{
    List<PulldownItemUIView> _uis;
    List<int> valueRef { get; set; }

    public override void Init(PulldownListUIModel element)
    {
        _uis = new List<PulldownItemUIView>();
        valueRef = new List<int>();
        base.Init(element);
        element.ChildUIModel.Parent = transform;
        for (var count = 0; count < element.Length; count++)
        {
            var c = count;
            valueRef.Add(0);
            element.ChildUIModel.Callback = num =>
            {
                valueRef[c] = count;
            };
            _uis.Add(Stream.Render<PulldownItemUIView, PulldownItemUIModel>(element.ChildUIModel));
            
        }
    }
    public List<int> Getvalues()
    {
        return valueRef;
    }

   
}
