using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public class ObjectPool
{
    public class PoolObject
    {
        public GameObject Obj { get; set; }
        public bool Using { get; set; }
        public void Destroy()
        {
            GameObject.Destroy(Obj);
            Using = false;
        }
        public void Reset()
        {

        }
    }
    public PoolObject[] Objects;
    public GameObject GetObject()
    {
        try
        {
            var target = Objects.First(_ => _.Using == false);
            target.Using = true;
            return target.Obj;
        }
        catch {
            Debug.Log("No Object");
            return null;
        }
    }
    public void Unuse(GameObject Obj)
    {//疑似的にハッシュ探索
        var index = -1 ;
        int.TryParse(Obj.name, out index);
        if(index == -1)
        {
            return;
        }
        Objects[index].Using = false;
        Objects[index].Reset();
    }
    public IEnumerator AllDestroy(int insPerFrame, Action stepAction = null)
    {
        var count = 0;
        for (int i = 0; i < Objects.Length; i++)
        {
            if (Objects[i] != null)
            {
                var o = Objects[i];
                o.Destroy();
                Objects[i] = null;
            }
            count++;
            if (count >= insPerFrame)
            {
                stepAction?.Invoke();
                count = 0;
                yield return null;
            }
        }
        Objects = null;
    }
    public IEnumerator InstantiatePrefab(GameObject _prefab ,int poolNum, int insPerFrame, Action stepAction = null)
    {

        Objects = new PoolObject[poolNum];
        var count = 0;
        for (int i = 0; i < poolNum; i++)
        {
            Objects[i] = new PoolObject();
            Objects[i].Obj = GameObject.Instantiate(_prefab);
            Objects[i].Obj.name = i.ToString();
            count++;
            if (count >= insPerFrame)
            {
                stepAction?.Invoke();
                count = 0;
                yield return null;
            }
        }
    }
    public IEnumerator InstantiateAction(Func<GameObject> create, int poolNum, int insPerFrame, Action stepAction = null)
    {

        Objects = new PoolObject[poolNum];
        var count = 0;
        for (int i = 0; i < poolNum; i++)
        {
            Objects[i] = new PoolObject();
            Objects[i].Obj = create();
            Objects[i].Obj.name = i.ToString();
            count++;
            if (count >= insPerFrame)
            {
                stepAction?.Invoke();
                count = 0;
                yield return null;
            }
        }
    }
    public IEnumerator InstantiateSphere(int poolNum, int insPerFrame, Action stepAction = null)
    {

        Objects = new PoolObject[poolNum];
        var count = 0;
        for (int i = 0; i < poolNum; i++)
        {
            Objects[i] = new PoolObject();
            Objects[i].Obj = CreateSphere(new Vector2(0, 0), 1);
            Objects[i].Obj.name = i.ToString();
            count++;
            if (count >= insPerFrame)
            {
                stepAction?.Invoke();
                count = 0;
                yield return null;
            }
        }
    }
    private GameObject CreateSphere(Vector2 pos, float rad)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.position = pos;
        go.transform.localScale = Vector3.one * rad;
        return go;
    }

}
