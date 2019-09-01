using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputLocal : InputBase
{
    public override IEnumerator WaitForButton(string text, Vector2 pos = default, Action cb = null)
    {
        pos = pos == null ? Vector2.zero : pos;
        using (var ui = new UIStream())
        {
            var button = ui.Render<ButtonUI, ButtonUIModel>(new ButtonUIModel()
            {
                PrefabPath = PrefabModel.Path.Button,
                Label = text,
                Position = pos
            });
            yield return button.WaitForClick();
        }
        cb?.Invoke();
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
    public override IEnumerator WaitForSelect(string[] list , Action<int> cb)
    {
        using (var ui = new UIStream())
        {
            var select = ui.Render<SelectListUi, SelectUIModel>(new SelectUIModel()
            {
                Labels = list,
                Callback = cb,
                PrefabPath = PrefabModel.Path.VerticalSelectList,
                ChildUIModel = new SelectItemUIModel()
                {
                    PrefabPath = PrefabModel.Path.VerticalSelectItem,
                }

            });
            yield return select.WaitForSelect();
        }

    }

    public override IEnumerator WaitForSelectMap(Action<Vector2Int> cb = null)
    {
        while (true) {
            if (Input.GetMouseButtonDown(0))
            {
                var pos = CameraManager.Instance.GameCamera.ScreenToWorldPoint(Input.mousePosition);
                var tile = MapUIManager.Instance.WorldToTilePos(pos);
                cb(tile);
                break;
            }
            yield return null;
        }
        
       
    }
}
