using System.Collections.Generic;
using UnityEngine;

public class UnityObjectPool<T> where T: MonoBehaviour
{
    private readonly T _prefab;
    private readonly Queue<T> _pool;
    private readonly Transform _container;
    private readonly bool _expansible;
    private readonly bool _isContainerNotNull;

    public UnityObjectPool (T prefab, Transform container = null, int capacity = 50, bool expand = true)
    {
        _prefab = prefab;
        _pool = new Queue<T>(capacity);
        _container = container;
        _expansible = expand;

        _isContainerNotNull = container != null;

        for (var i = 0; i < capacity; i++)
            CreateInstance();
    }

    private void CreateInstance()
    {
        var instance = _isContainerNotNull ? Object.Instantiate(_prefab, _container) : Object.Instantiate(_prefab);
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);
    }

    public T GetInstance()
    {

        if (_pool.Count == 0)
        {
            if (_expansible)
                CreateInstance();
            else
                return null;
        }
        var instance = _pool.Dequeue();
        instance.gameObject.SetActive(true);
        return instance;
    }

    public void ReleaseInstance(T instance)   
    {
        instance.gameObject.SetActive(false);
        _pool.Enqueue(instance);
    }

}

