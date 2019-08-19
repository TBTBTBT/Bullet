using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public static class IEnumeratorExtensions
{
    public static IEnumerator Paralell(this IEnumerator func, params IEnumerator[] funcs)
    {
        List<IEnumerator> Currents = new List<IEnumerator>() { func };
        Currents.AddRange(funcs.ToList());
        while (Currents.Count > 0)
        {
            Currents.RemoveAll(_ => !_.MoveNext());
            yield return null;
        }

    }
}