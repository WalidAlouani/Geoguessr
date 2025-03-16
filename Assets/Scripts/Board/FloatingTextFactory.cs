using System.Collections.Generic;
using UnityEngine;

public class FloatingTextFactory : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private UI_FloatingText prefab;

    private IPoolingSystem<UI_FloatingText> pool;

    private void Start()
    {
        pool = new ObjectPool<UI_FloatingText>(prefab, container);
    }

    public UI_FloatingText Create(Vector3 position, int value)
    {
        var obj = pool.Get();
        obj.transform.position = position;
        obj.Init(value);
        return obj;
    }

    private void OnDestroyed(UI_FloatingText obj)
    {
        pool.Return(obj);
    }
}

