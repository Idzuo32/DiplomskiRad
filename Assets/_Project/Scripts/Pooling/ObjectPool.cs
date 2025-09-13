using System.Collections.Generic;
using UnityEngine;


public interface IPoolable
{
  void OnPool();
  void OnRelease();
}


public class ObjectPool : MonoBehaviour
{
  [SerializeField] GameObject prefabToPool;
  [SerializeField] int initialSize = 10;


  readonly Queue<GameObject> pool = new();


  public void Initialize()
  {
    ExpandPool(initialSize);
  }


  void ExpandPool(int count)
  {
    for (var i = 0; i < count; i++)
    {
      ProcessPoolExpansion();
    }
  }

  void ProcessPoolExpansion()
  {
    var instantiatedGameObject = Instantiate(prefabToPool, transform);
    instantiatedGameObject.SetActive(false);
    pool.Enqueue(instantiatedGameObject);
  }


  public GameObject Pool()
  {
    ExpandPoolIfEmpty();
    return ProcessedPool();
  }

  void ExpandPoolIfEmpty()
  {
    if (pool.Count == 0)
    {
      ExpandPool(1);
    }
  }

  GameObject ProcessedPool()
  {
    var pooledGameObject = pool.Dequeue();
    pooledGameObject.SetActive(true);
    HandlePool(pooledGameObject);
    return pooledGameObject;
  }

  void HandlePool(GameObject pooledGameObject)
  {
    pooledGameObject.GetComponent<IPoolable>()?.OnPool();
  }

  public void Release(GameObject pooledGameObject)
  {
    HandleRelease(pooledGameObject);
    pooledGameObject.SetActive(false);
    pool.Enqueue(pooledGameObject);
  }


  static void HandleRelease(GameObject pooledGameObject)
  {
    pooledGameObject.GetComponent<IPoolable>()?.OnRelease();
  }
}