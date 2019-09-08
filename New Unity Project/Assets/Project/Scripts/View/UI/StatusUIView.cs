using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class StatusUIView : UIViewBase<StatusModel>
{

    [SerializeField] TMP_Text Name;
    [SerializeField] TMP_Text Hp;
    [SerializeField] TMP_Text Atk;
    [SerializeField] TMP_Text Def;
    [SerializeField] TMP_Text Spd;
    [SerializeField] TMP_Text Mag;
    public void SetView()
    {

    }


    public override void Init(StatusModel element)
    {
        base.Init(element);
        Name.text = element.Name;
        Hp.text = $"HP : {element.HpCurrent.ToString()} / {element.Hp.ToString()}";
        Atk.text = "Atk : " + element.Atk.ToString();
        Def.text = "Def : " + element.Def.ToString();
        Spd.text = "Spd : " + element.Spd.ToString();
        Mag.text = "Mag : " + element.Mag.ToString();
    }

}

