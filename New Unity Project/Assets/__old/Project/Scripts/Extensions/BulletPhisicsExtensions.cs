using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletPhisicsExtensions
{
    public static void SetElement(this BulletPhysics self,Vector2 pos,float angle,int atkId,int frame)
    {
        var attack = MasterdataManager.Get<MstAttackRecord>(atkId);
        var record = MasterdataManager.Get<MstBulletRecord>(attack.bulletId);
        var bullet = BulletElementMaker.Create();
        bullet.Start.Pos.Cur = pos;
        bullet.Start.Angle.Cur = angle;
        var radian = angle * Mathf.Deg2Rad;
        Debug.Log(new Vector2(record.velH * Mathf.Cos(radian), record.velV * Mathf.Sin(radian)));
        bullet.Start.Pos.Vel = 
              new Vector2(record.velV * Mathf.Cos(radian), record.velV * Mathf.Sin(radian))
            + new Vector2(record.velH * Mathf.Sin(radian), record.velH * Mathf.Cos(radian));
        bullet.Start.Pos.Acc = new Vector2(record.accH*Mathf.Cos(radian),record.accV * Mathf.Sin(radian));
        bullet.Start.Radius.Cur = record.radius;
        bullet.Start.Radius.Vel = record.radiusVel;
        bullet.Start.Radius.Acc = record.radiusAcc;

        bullet.Frame = frame;
        bullet.EndFrame = frame + record.lifeTimeFrame;
        
        self.SetElement(bullet);
    }
    
}
