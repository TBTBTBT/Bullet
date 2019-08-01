using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

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
        //var e = new BulletElementInfo { Input = element };
        IdCount++;
        //e.Id = IdCount;
        //e.EndFrame = -1;
        _inputs.Add(element);
        if (element.Frame < Time)
        {//計算し直し
            ReCalc(element.Frame,FrameCount);
        }
        //_elements.Add(e);
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
        CheckActive();
        PreMove();
        Physics.Simulate(Span);

    }
    private void SetTime(int frame)
    {
        Time = Span * frame;
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
        var isActive = Info.Frame >= currentFrame && endFrame >= currentFrame;
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
}