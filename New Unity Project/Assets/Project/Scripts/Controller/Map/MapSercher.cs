using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class MapSercher
{
    public List<(Vector2Int pos, int[] dir)> Result = new List<(Vector2Int pos, int[] dir)>();
    public UnityEvent OnUpdateResult = new UnityEvent();
    /// <summary>
    /// いけますよ
    /// </summary>
    /// <param name="pos">位置</param>
    /// <param name="num">さいころの目</param>
    /// <returns></returns>
    public IEnumerator Search(int[,] map, Vector2Int pos, int num)
    {
        Result.Clear();
        var count = 0;
        var beforeLength = 0;
        foreach (var dir in GetDirection(num))
        {
            var result = CanReach(ref map, pos, dir);
            if (result == Vector2Int.one * -1)
            {//到達不可能
                continue;
            }
            if (Result.Any(r => r.pos == result))
            {
                continue;
            }
            Result.Add((result, dir));
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

    private IEnumerable<int[]> GetDirection(int num)
    {
        //組み合わせ総数
        var allNum = Math.Pow(4, num);
        var list = Enumerable.Repeat<int>(0, num).ToArray();
        for (int i = 0; i < allNum; i++)
        {

            yield return list;
        }
    }
    private Vector2Int CanReach(ref int[,] map, Vector2Int pos, int[] directions)
    {
        return Vector2Int.one * -1;
    }
}