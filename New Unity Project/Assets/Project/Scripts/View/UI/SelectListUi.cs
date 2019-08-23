using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectListUi : UIViewBase<SelectUIModel>
{
    [SerializeField] private RectTransform _parent;

    public RectTransform Parent => _parent;

}
