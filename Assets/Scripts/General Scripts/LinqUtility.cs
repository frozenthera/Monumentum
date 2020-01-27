using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class LinqUtility
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
            action(item);
    }

    public static void ForEachWithCasting<T, T2>(this IEnumerable<T> source, Action<T2> action)
    {
        foreach (var item in source)
            if(item is T2 cast)
                action(cast);
    }

    /*public static T2 ForEachWithCasting<T, T2>(this IEnumerable<T> source, Func<T2> action)
    {
        foreach (var item in source)
            if (item is T2 cast)
                return action();
        return default;
    }*/
}
