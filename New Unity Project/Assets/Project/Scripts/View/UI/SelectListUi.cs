using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectListUi : UIViewBase<SelectUIModel>
{
    [SerializeField] private RectTransform _parent;

    public RectTransform Parent => _parent;
    private bool _onSelect = false;
    public override void Init(SelectUIModel element)
    {
        base.Init(element);
        element.ChildUIModel.Parent = transform;
        for (var count = 0; count < element.Labels.Length; count++)
        {
            var c = count;
            element.ChildUIModel.Callback = () =>
            {
                element.Callback?.Invoke(c);
                _onSelect = true;
            };
            element.ChildUIModel.Label = element.Labels[c];
            Stream.Render<SelectListItemUi, SelectItemUIModel>(element.ChildUIModel);

        }

    }
    public IEnumerator WaitForSelect()
    {
        while (!_onSelect)
        {
            yield return null;
        }
    }
}
