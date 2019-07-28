using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[Serializable]
public struct FloatPhisicsSet
{
    public float Cur;
    public float Vel;
    public float Acc;

}
[Serializable]
public struct Vector2PhisicsSet
{
    public Vector2 Cur;
    public Vector2 Vel;
    public Vector2 Acc;

}

[Serializable]
public struct BulletElementSet
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
public struct BulletElementInput
{
    public int Frame;
    public BulletElementSet Start;
    public Func<BulletPhisics, Vector2> OriginalMove;
}
[Serializable]
public struct BulletElementInfo
{
    public int Id;
    public BulletElementInput Input;
    public int EndFrame;//最後の生存F

    public BulletElementSet Current;
}
[Serializable]
public struct BulletElementResult
{
    public int CurrentFrame;
    public bool IsActive;
    public BulletElementSet Result;
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

    public static bool IsActive(this BulletElementInfo self,int frame)
    {
        return self.Input.Frame <= frame && ( self.EndFrame == -1 || frame <= self.EndFrame );
    }

    public static Vector2PhisicsSet CalcOnce(this Vector2PhisicsSet self,float span)
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
[Serializable]
public class BulletPhisics
{

    private List<BulletElementInfo> _elements = new List<BulletElementInfo>();
    public List<BulletElementInfo> Elements => _elements;
    public int FrameCount { get; private set; }
    public float Time { get; private set; }
    public float Span { get; private set; }
    public int IdCount {get;set;} = 0;
    //private List<SnapShot<BulletPhisics>> _snapShots = new List<SnapShot<BulletPhisics>>();
    #region Public

    public void Reset()
    {
        _elements.Clear();
        FrameCount = 0;
        Time = 0;
        IdCount = 0;
    }
    public void SetElement(BulletElementInput element)
    {
        var e = new BulletElementInfo { Input = element };
        IdCount++;
        e.Id = IdCount;
        e.EndFrame = -1;
        if (element.Frame < Time)
        {//計算し直し
            ReCalc(element.Frame,FrameCount);
        }
        _elements.Add(e);
    }

    public void SetTimeSpan(float span)
    {
        Span = span;
    }
    public void RollBack(int frame)
    {
        FrameCount = frame;
        SetTime(FrameCount);
        //todo スナップショットから目的時間以前の一番最新の状態を復元

    }
    //public void RollForward(int frame)
    //{

    //}

    public void ReCalc(int checkPoint, int now)
    {
        RollBack(checkPoint);
        while (FrameCount < now)
        {
            Simulate();
        }
    }
    public void Simulate()
    {
        FrameCount++;
        SetTime(FrameCount);
        CalcOnce();
    }
    #endregion
  
    //計算
    private void CalcOnce()
    {
        PreMove();
        HitCheck();
        ResolveCollision();
        AfterMove();
    }

    private void PreMove()
    {
        foreach (var bulletElementInfo in _elements)
        {
            if (!bulletElementInfo.IsActive(FrameCount))
            {
                continue;
            }
            //bulletElementInfo.Input.OriginalMove
            bulletElementInfo.Current.Pos.CalcOnce(Span);
        }
    }

    private void HitCheck()
    {

    }

    private void ResolveCollision()
    {

    }

    private void AfterMove()
    {

    }
    //汎用
    private void SetTime(int frame)
    {
        Time = Span * frame;
    }
    private void TakeSnapShot()
    {
    //    _snapShots.Add(new SnapShot<BulletPhisics>(this));
    }
}
