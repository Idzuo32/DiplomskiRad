using System.Collections.Generic;
using UnityEngine;


public interface IPoolable
{
    void OnSpawn();
    void OnDespawn();
}


public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] int initialSize = 10;


    readonly Queue<GameObject> pool = new();


    void Awake()
    {
        Expand(initialSize);
    }


    void Expand(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }


    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            Expand(1);
        }

        GameObject obj = pool.Dequeue();
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawn();
        return obj;
    }


    public void Release(GameObject obj)
    {
        obj.GetComponent<IPoolable>()?.OnDespawn();
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}