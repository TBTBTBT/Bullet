using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletPhisicsExtensions
{
    public static void SetElement(this BulletPhysics self,Vector2 pos,int atkId)
    {
        var attack = MasterdataManager.Get<MstAttackRecord>(atkId);
        var bullet = BulletElementMaker.Create();
        bullet.Start.Pos.Cur = pos;
        
        self.SetElement(bullet);
    }
    
}
