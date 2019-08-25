using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputNetwork : InputBase
{
    public override IEnumerator WaitForButton(string text, Vector2 pos = default, Action cb = null)
    {
        throw new NotImplementedException();
    }

    public override IEnumerator WaitForSelect<T>(Action<T> cb)
    {
        using (var ui = new UIStream())
        {
            var select = ui.Render<SelectListUi, SelectUIModel>(new SelectUIModel()
            {
                PrefabPath = PrefabModel.Path.VerticalSelectList,
                ChildUIModel = new SelectItemUIModel()
                {
                    PrefabPath = PrefabModel.Path.VerticalSelectItem,
                }
            }.FromEnum<T>(cb));
            yield return select.WaitForSelect();
        }

    }

}
