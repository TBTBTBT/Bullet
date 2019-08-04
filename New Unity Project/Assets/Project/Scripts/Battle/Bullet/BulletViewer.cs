using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletViewer : MonoBehaviour
{


    public LoadingState Loading { get; private set; } = new LoadingState();
    //private ObjectPool Objects { get; set; }
    private Dictionary<int, GameObject> _managedObject = new Dictionary<int, GameObject>();
    //private void Awake()
    //{
    //    Objects = new ObjectPool();
    //}
    //public IEnumerator InitAsync(int poolNum, int stepPerFrame)
    //{
       
    //    Loading.CurrentStep = 0;
    //    Loading.MaxStep = poolNum / stepPerFrame;
    //    if (Objects.Objects != null)
    //    {
    //        Loading.MaxStep += Objects.Objects.Length / stepPerFrame;
    //        yield return Objects.AllDestroy(stepPerFrame, () => Loading.CurrentStep++);
    //    }
    //    yield return Objects.InstantiateSphere(poolNum, stepPerFrame, () => Loading.CurrentStep++);
    //}

    public void UpdateView(BulletPhysics p)
    {
        var remain  = new List<int>(_managedObject.Keys);

        foreach (var e in p.Elements)
        {
            if (_managedObject.ContainsKey(e.Id))
            {
                Debug.Log(e.Current.Pos.Cur);
                _managedObject[e.Id].transform.position = e.Current.Pos.Cur;
            }
            else
            {
                _managedObject.Add(e.Id, CreateSphere(e.Current.Pos.Cur, e.Current.Radius.Cur));
            }
            remain.Remove(e.Id);
            //    if (_managedObject.ContainsKey(e.Id))
            //    {
            //        //オブジェクトの更新
            //        _managedObject[e.Id].transform.localPosition = e.Current.Pos.Cur;

            //        if (!e.IsActive(p.FrameCount))
            //        {   //アクティブではないがオブジェクトがある
            //            Objects.Unuse(_managedObject[e.Id]);
            //            _managedObject.Remove(e.Id);
            //        }


            //    }
            //    else if (e.IsActive(p.FrameCount))
            //    {
            //        //アクティブでオブジェクトがない
            //        _managedObject.Add(e.Id, Objects.GetObject());
            //    }
        }
       
        foreach(var r in remain)
        {
            Destroy(_managedObject[r]);
            _managedObject.Remove(r);
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
