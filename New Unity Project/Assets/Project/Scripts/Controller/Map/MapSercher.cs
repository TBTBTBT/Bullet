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
        foreach (var reach in CanReach(map,pos,num))
        {

            if (Result.Any(r => r == reach))
            {
                continue;
            }
            Result.Add(reach);
            count++;
            if (count > 1000)
            {
                if (beforeLength != Result.Count)
                {
                    OnUpdateResult?.Invoke();
                }
                beforeLength = Result.Count;
                yield return null;
                count = 0;
            }
        }

        yield return null;
    }

    private IEnumerable<int> CanReach(StationModel[] map, int start,int length,int before = -1)
    {
        if (length == 0)
        {
            yield return start;
        }
        foreach (var r in map[start].Relation)
        {
            if (r != before)
            {
                foreach (var reach in CanReach(map, r, length - 1, start))
                {
                    yield return reach;
                }
            }

        }
    }

}