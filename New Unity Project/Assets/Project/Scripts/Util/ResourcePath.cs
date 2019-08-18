using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
[Serializable]
[AttributeUsage(AttributeTargets.Field, Inherited = false)]
public class ResourcePath : Attribute
{
    public string Path;

    public ResourcePath(string path)
    {
        Path = path;
    }
}
public static class EnumExtensions
{
    /// <summary>
    /// 特定の属性を取得する
    /// </summary>
    /// <typeparam name="TAttribute">属性型</typeparam>
    public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
    {
        //リフレクションを用いて列挙体の型から情報を取得
        var fieldInfo = value.GetType().GetField(value.ToString());
        //指定した属性のリスト
        var attributes
            = fieldInfo.GetCustomAttributes(typeof(TAttribute), false)
            .Cast<TAttribute>();
        //属性がなかった場合、空を返す
        if ((attributes?.Count() ?? 0) <= 0)
            return null;
        //同じ属性が複数含まれていても、最初のみ返す
        return attributes.First();
    }
}