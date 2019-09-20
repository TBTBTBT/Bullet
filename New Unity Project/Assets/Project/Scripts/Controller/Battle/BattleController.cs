using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController
{
    private List<BattleCharaModel> chara;
    public BattleCharaModel enemy;
    public void SetPlayer(BattleCharaModel m)
    {
        if (chara.Count < 2)
        {
            chara.Add(m);
        }

    }

    public void Clear()
    {
        chara.Clear();
    }
    public List<BattleCharaModel> Chara => chara;

}
