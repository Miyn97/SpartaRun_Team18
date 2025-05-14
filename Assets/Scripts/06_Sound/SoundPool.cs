using System.Collections.Generic;
using UnityEngine;

public class SoundPool
{
    private readonly GameObject prefab;
    private readonly Transform parent;
    private readonly Queue<SoundSource> pool = new();
    private readonly List<SoundSource> active = new List<SoundSource>();

    public SoundPool(GameObject prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public IEnumerable<SoundSource> ActiveSources => active; // 외부(SoundManager)에서 활성 소스를 열람 가능하도록 유도

    public SoundSource Get(bool loop, float volume)
    {
        SoundSource source = pool.Count > 0 ? pool.Dequeue() : CreateNew();
        source.Init(loop, volume);
        source.gameObject.SetActive(true);

        active.Add(source);
        return source;
    }

    public void Return(SoundSource source)
    {
        source.Stop();
        source.gameObject.SetActive(false);

        active.Remove(source);
        pool.Enqueue(source);
    }

    private SoundSource CreateNew()
    {
        GameObject go = Object.Instantiate(prefab, parent);
        return go.GetComponent<SoundSource>();
    }
}