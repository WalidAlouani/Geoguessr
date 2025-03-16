using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : IPoolingSystem<T> where T : Component, IPoolable
{
    private readonly Queue<T> pool = new Queue<T>();
    private readonly T prefab;
    private readonly Transform parent;

    public ObjectPool(T prefab, Transform parent)
    {
        this.prefab = prefab;
        this.parent = parent;
    }

    public T Get()
    {
        T obj;
        if (pool.Count > 0)
        {
            obj = pool.Dequeue();
            obj.gameObject.SetActive(true);
        }
        else
        {
            obj = GameObject.Instantiate(prefab, parent);
            obj.Return = (T) => { Return(obj); };
        }
        obj.OnCreate();
        return obj;
    }

    public void Return(T obj)
    {
        obj.OnRelease();
        obj.gameObject.SetActive(false);
        pool.Enqueue(obj);
    }
}
