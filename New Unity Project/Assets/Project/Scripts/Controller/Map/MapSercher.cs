using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapSercher
{
    public List<int> Result = new List<int>();
    public UnityEvent OnUpdateResult = new UnityEvent();
    /// <summary>
    /// いけますよ
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="num">さいころの目</param>
    /// <returns></returns>

    public IEnumerator Search(StationModel[] map, int pos, int num)
    {
        Result.Clear();
        var count = 0;
        var beforeLength = 0;
        //foreach (var reach in CanReach(map,pos,num))
        //{

        //    if (Result.Any(r => r == reach))
        //    {
        //        continue;
        //    }
        //    Result.Add(reach);
        //    count++;
        //    if (count > 1000)
        //    {
        //        if (beforeLength != Result.Count)
        //        {
        //            OnUpdateResult?.Invoke();
        //        }
        //        beforeLength = Result.Count;
        //        yield return null;
        //        count = 0;
        //    }
        //    yield return null;
        //}
        var list = new List<(int before, List<int> list)>() { (-1,new List<int>() { pos}) };
        while(num > 0)
        {
            Debug.Log(num);
            var nextList = new List<(int before, List<int> list)>();
            foreach (var l in list) {
                Debug.Log("l : " +l.before);
                foreach (var l2 in l.list)
                {
                    nextList.Add(CanReach(map, l2, num, l.before));
                    Debug.Log("l2 : " + nextList.Count);
                    yield return null;
                }
                yield return null;

            }
            list.Clear();
            list = nextList;
            num = num - 1;
            yield return null;
        }
        foreach(var l in list)
        {
            Result.Add(l.before);
        }
        
        yield return null;
    }
    private (int before, List<int> list) CanReach(StationModel[] map, int start, int length, int before = -1)
    {
        if (length == 0)
        {
            return (start,new List<int>() { start });
        }
        Debug.Log(length);
        var list = new List<int>();
        foreach (var r in map[start].Relation)
        {
            if (r != before)
            {
                list.Add(r);
            }

        }
        return (start,list);
    }
    //private IEnumerable<int> CanReach(StationModel[] map, int start,int length,int before = -1)
    //{
    //    if (length == 0)
    //    {
    //        yield return start;
    //    }
    //    Debug.Log(length);
    //    foreach (var r in map[start].Relation)
    //    {
    //        if (r != before)
    //        {
    //            foreach (var reach in CanReach(map, r, length - 1, start))
    //            {
    //                yield return reach;
    //            }
    //        }

    //    }
    //}

}