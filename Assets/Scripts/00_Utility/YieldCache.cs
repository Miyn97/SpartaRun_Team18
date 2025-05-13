using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaitForSeconds 캐싱을 통해 GC 할당을 최소화하는 유틸리티 클래스
/// </summary>
public static class YieldCache
{
    private static readonly Dictionary<float, WaitForSeconds> cache = new();

    public static WaitForSeconds WaitForSeconds(float time)
    {
        if (!cache.ContainsKey(time))
            cache[time] = new WaitForSeconds(time);

        return cache[time];
    }
}
