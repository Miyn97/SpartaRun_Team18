using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// WaitForSeconds ĳ���� ���� GC �Ҵ��� �ּ�ȭ�ϴ� ��ƿ��Ƽ Ŭ����
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
