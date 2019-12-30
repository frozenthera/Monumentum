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
}
