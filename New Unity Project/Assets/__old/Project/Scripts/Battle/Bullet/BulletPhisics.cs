#define DEBUG
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Linq;
public class BulletPhysics
{
    private List<BulletElementInput> _inputs = new List<BulletElementInput>();
    public List<BulletElementInfo> Elements = new List<BulletElementInfo>();
    private List<CharacterElementInput> _charaInput = new List<CharacterElementInput>();
    public List<CharacterElementInfo> CharaElements = new List<CharacterElementInfo>();
    //private List<BulletElementInfo> ElementsLog = new List<BulletElementInfo>();
    public int FrameCount { get; private set; }
    public float NowTime { get; private set; }
    public float Span { get; private set; }
    public int IdCount { get; set; } = 0;
    public int CharaIdCount { get; set; } = 0;
    #region Infomation
    public int ElementsCount => Elements.Count;
    public int ActiveElements => Elements.Count(_=>_.IsActive(FrameCount));

    #endregion
    public void Reset()
    {
        Elements.Clear();
        FrameCount = 0;
        NowTime = 0;
        IdCount = 0;
        CharaIdCount = 0;
    }
    public void SetTimeSpan(float span)
    {
        Span = span;
    }
    public void SetCharacter(CharacterElementInput element)
    {
       
        _charaInput.Add(element);
        
    }
    public void SetElement(BulletElementInput element)
    {

        _inputs.Add(element);
        if (element.Frame < NowTime)
        {//計算し直し
            RecalcAll(FrameCount);
            //ReCalc(element.Frame, FrameCount);
        }
    }
    /// <summary>
    /// 最初のフレームから計算しなおし
    /// </summary>
    /// <param name="now"></param>
    public void RecalcAll(int now)
    {
        var time = Time.time;
        Reset();
        while (FrameCount < now)
        {
            Simulate();
        }
        Debug.Log($"Recalc elapse : {Time.time - time}");

    }
#if false
    /// <summary>
    /// 未完成
    /// </summary>
    /// <param name="checkPoint"></param>
    /// <param name="now"></param>
    public void ReCalc(int checkPoint, int now)
    {
        RollBack(checkPoint);
        while (FrameCount < now)
        {
            Simulate();
        }
    }
        public void RollBack(int frame)
    {
        FrameCount = frame;
        SetTime(FrameCount);
        //todo スナップショットから目的時間以前の一番最新の状態を復元

    }
#endif
    /// <summary>
    /// 1フレーム進める
    /// </summary>
    public void Simulate()
    {
        FrameCount++;
        SetTime(FrameCount);
        CalcOnce();
    }
    public void Initialize()
    {
        Reset();
        AddActiveChara();

    }
    private void SetTime(int frame)
    {
        NowTime = Span * frame;
    }
    private void CalcOnce()
    {
        
        PreMove();
        AddActiveElement();
        DeleteDisableElement();
        CalcCollision();
    }
    #region Calc
    private void PreMove()
    {
        foreach(var e in Elements)
        {
            e.Current.Pos.Cur += e.Current.Pos.Vel * Span;
            e.Current.Pos.Vel += e.Current.Pos.Acc * Span;
        }
    }
    private void PreMoveChara()
    {
        foreach (var e in CharaElements)
        {
            e.Current.Pos.Cur += e.Current.Pos.Vel * Span;
            e.Current.Pos.Vel += e.Current.Pos.Acc * Span;

        }
    }
    private void CalcCollision()
    {
        foreach (var e in Elements)
        {
            //e.Current.Pos.CalcOnce(Span);
            foreach (var e2 in Elements)
            {
                if(e.Id == e2.Id)
                {
                    continue;
                }
                var mag = (e.Current.Pos.Cur - e2.Current.Pos.Cur).magnitude;
                var rad = e.Current.Radius.Cur + e2.Current.Radius.Cur;
                if (mag < rad)
                {

                    //hit
                    Debug.Log("Hit");
                }
            }
        }
    }
    /// <summary>
    /// inputの中でアクティブになりうるものをアクティブにする
    /// </summary>
    private void AddActiveElement()
    {
        foreach(var i in _inputs)
        {
            if(i.Frame == FrameCount)
            {
                Elements.Add(new BulletElementInfo()
                {
                    Id = IdCount,
                    Input = i,
                    Current = i.Start,
                    EndFrame = i.EndFrame

                });
                IdCount++;
            }
        }
    }
    private void DeleteDisableElement()
    {
        Elements.RemoveAll(_ => _.EndFrame < FrameCount);
    }
    private void AddActiveChara()
    {
        foreach (var i in _charaInput)
        {

            CharaElements.Add(new CharacterElementInfo()
            {
                Id = CharaIdCount,
                Input = i,
                Current = i.Start,

            });
            CharaIdCount++;
            
        }
    }
    #endregion

}

/*
      [Serializable]
public class BulletPhysics
{

    private List<BulletElementInput> _inputs = new List<BulletElementInput>();
    //public List<BulletElementInfo> Elements => _elements;
    private GameObject _root;
    private ObjectPool Objects { get; set; }
    //private Dictionary<int, GameObject> _managedObject = new Dictionary<int, GameObject>();

    public int FrameCount { get; private set; }
    public float Time { get; private set; }
    public float Span { get; private set; }
    public int IdCount {get;set;} = 0;
    public int InputsCount => _inputs.Count;
    public int ActiveObjects => Objects?.Objects?.Count(_ => _ != null && _.Obj.activeInHierarchy) ?? 0;
    //private List<SnapShot<BulletPhisics>> _snapShots = new List<SnapShot<BulletPhisics>>();
#region Public
    public IEnumerator Init(int pool,int step,Action stepAction)
    {
        Physics.autoSimulation = false;
        _root = new GameObject();
        Objects = new ObjectPool();
        yield return Objects.InstantiateAction(CreateObject, pool, step, stepAction);
    }
    public void Reset()
    {
        //_elements.Clear();
        FrameCount = 0;
        Time = 0;
        IdCount = 0;
    }
    public void SetElement(BulletElementInput element)
    {
        IdCount++;
        _inputs.Add(element);
        if (element.Frame < Time)
        {//計算し直し
            ReCalc(element.Frame,FrameCount);
        }
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
        CheckActive();
        PreMove();
        Physics.Simulate(Span);

    }
    private void SetTime(int frame)
    {
        Time = Span * frame;
    }
    private void CheckInput()
    {
        foreach (var o in Objects.Objects)
        {
            o.Obj.GetComponent<BulletComponent>().CheckActivate(FrameCount);

        }
    }
    private void PreMove()
    {
        foreach(var o in Objects.Objects)
        {
            o.Obj.GetComponent<BulletComponent>().Move();

        }
        
    }
    private void CheckActive()
    {
        foreach (var o in Objects.Objects)
        {
            o.Obj.GetComponent<BulletComponent>().CheckActivate(FrameCount);

        }
    }

    //汎用
    private GameObject CreateObject()
    {
        var go = new GameObject();
        if (_root != null)
        {
            go.transform.parent = _root.transform;
        }
        go.AddComponent<BulletComponent>().Init();
        
        return go;
    }
   
    private void TakeSnapShot()
    {
    //    _snapShots.Add(new SnapShot<BulletPhisics>(this));
    }
}
class BulletComponent : MonoBehaviour
{
    Rigidbody2D Rigidbody;
    Collider2D Colider;
    GameObject _aim;
    BulletElementInput Info;
    Vector2 _collisionResult = new Vector2(0,0);
    bool _enableCollision = false;
    int endFrame = int.MaxValue;
    public void Init()
    {

        var rg = gameObject.AddComponent<Rigidbody2D>();
        var cc = gameObject.AddComponent<CircleCollider2D>();
        rg.gravityScale = 0;
        cc.isTrigger = true;
        Rigidbody = rg;
        Colider = cc;
        gameObject.transform.position = new Vector3(0, 0, 0);
#if DEBUG
        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.parent = gameObject.transform;
        sphere.transform.position = Vector3.zero;
#endif
        gameObject.SetActive(false);
    }
    public void Set(BulletElementInput info)
    {
        Info = info;
    }
    public void SetAim(GameObject aim)
    {
        _aim = aim;
    }
    public void CheckActivate(int currentFrame)
    {
        if(Info == null)
        {
            return;
        }
        var isActive =  currentFrame >= Info.Frame && endFrame >= currentFrame;
        gameObject.SetActive(isActive);

    }
    public void Move()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }
        Info.Start.Pos.Vel += Info.Start.Pos.Acc;
        if(_aim != null)
        {
            //追尾
        }
        if (_enableCollision)
        {
            Info.Start.Pos.Vel += _collisionResult;
            _collisionResult = Vector2.zero;
        }
        Rigidbody.velocity = Info.Start.Pos.Vel;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        PushBack(collision);
    }
    void PushBack(Collider2D c)
    {

        var rg = c.gameObject.GetComponent<Rigidbody2D>();
        var dist = (c.transform.position - gameObject.transform.position).normalized;
        _collisionResult = _collisionResult + (Vector2)dist * 10;
    }
}*/
