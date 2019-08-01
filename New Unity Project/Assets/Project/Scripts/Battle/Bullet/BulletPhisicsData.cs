using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public class FloatPhisicsSet
{
    public float Cur;
    public float Vel;
    public float Acc;

}
[Serializable]
public class Vector2PhisicsSet
{
    public Vector2 Cur;
    public Vector2 Vel;
    public Vector2 Acc;

}

[Serializable]
public class BulletElementSet
{
    public Vector2PhisicsSet Pos;//位置
    public FloatPhisicsSet Angle;//向き
    public FloatPhisicsSet Radius;
    public int Strength;//貫通力
    public int Endurance;//耐久値 Strength比較で同値か低い方-1 0になると破棄
    public int Attack;
    public int AttackAdditional;

}
[Serializable]
public class BulletElementInput
{
    public int Frame;
    public BulletElementSet Start;
    public Func<BulletPhysics, Vector2> OriginalMove;
}
[Serializable]
public class BulletElementInfo
{
    public int Id;
    public BulletElementInput Input;
    public int EndFrame;//最後の生存F

    public BulletElementSet Current;
}
[Serializable]
public class BulletElementResult
{
    public int CurrentFrame;
    public bool IsActive;
    public BulletElementSet Result;
}
public static class BulletElementMaker
{

    public static BulletElementInput Create()
    {
        BulletElementInput ret = new BulletElementInput();
        ret.Start = new BulletElementSet()
        {
            Pos = new Vector2PhisicsSet()
            {
                Cur = Vector2.zero,
                Acc = Vector2.zero,
                Vel = Vector2.zero
            },
            Angle = new FloatPhisicsSet()
            {
                Cur = 0,
                Acc = 0,
                Vel = 0
            },
            Radius = new FloatPhisicsSet()
            {
                Cur = 0,
                Acc = 0,
                Vel = 0,
            },
            Endurance = 0,
            Strength = 0,
            Attack = 0,
            AttackAdditional = 0,



        };

        return ret;
    }
}

public static class Extensions
{
    //public static T DeepCopy<T>(this T src)
    //{
    //    using (var memoryStream = new MemoryStream())
    //    {
    //        var binaryFormatter = new BinaryFormatter();
    //        binaryFormatter.Serialize(memoryStream, src);
    //        memoryStream.Seek(0, System.IO.SeekOrigin.Begin);
    //        return (T)binaryFormatter.Deserialize(memoryStream);
    //    }
    //}

    public static bool IsActive(this BulletElementInfo self, int frame)
    {
        return self.Input.Frame <= frame && (self.EndFrame == -1 || frame <= self.EndFrame);
    }

    public static Vector2PhisicsSet CalcOnce(this Vector2PhisicsSet self, float span)
    {
        self.Cur += self.Vel * span;
        self.Vel += self.Acc * span;
        return self;
    }
}

//public class SnapShot<T> where T : class
//{
//    public T Data { get; set; }
//    public SnapShot(T data)
//    {
//        Data = data.DeepCopy();
//    }

//}