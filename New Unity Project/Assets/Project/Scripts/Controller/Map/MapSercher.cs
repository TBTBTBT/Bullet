using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Threading.Tasks;
public class MapSercher
{
    public List<int> Result = new List<int>();
    public UnityEvent OnUpdateResult = new UnityEvent();
    public List<StationModel> ResultToStation(StationModel[] map) {
        var list = new List<StationModel>();
        foreach (var r in Result)
        {
            list.Add(map[r]);
        }
        return list;
    }
    /// <summary>
    /// いけますよ
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="num">さいころの目</param>
    /// <returns></returns>

    public IEnumerator Search(StationModel[] map, int pos, int num)
    {
        Result.Clear();

        var list = new List<(int before, List<int> list)>() { (-1,new List<int>() { pos}) };
        var isFinished = false;
        Task.Run(()=>
        {
            SearchAsync(map, list, num);
            isFinished = true;
        });
        while (!isFinished)
        {
            yield return null;
        }
      
        
        yield return null;
    }
    private void SearchAsync(StationModel[] map, List<(int before, List<int> list)> list,int num)
    {

        while (num >= 0)
        {
            Debug.Log(num);
            var nextList = new List<(int before, List<int> list)>();
            foreach (var l in list)
            {
                Debug.Log("l : " + l.before);
                foreach (var l2 in l.list)
                {
                    nextList.Add(CanReach(map, l2, num, l.before));
                }

            }
            list.Clear();
            list = nextList;
            num = num - 1;
            
        }
        foreach (var l in list)
        {
            Result.Add(l.before);
        }
    }
    private (int before, List<int> list) CanReach(StationModel[] map, int start, int length, int before = -1)
    {
        if (length < 0)
        {
            return (start,new List<int>() { start });
        }
        Debug.Log("s :" +start+ ", rem" +length);
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