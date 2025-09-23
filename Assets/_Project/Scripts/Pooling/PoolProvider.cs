using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utilities;

public class PoolProvider : Singleton<PoolProvider> {
  [System.Serializable]
  public class PoolData {
    public string id;
    public ObjectPool objectPool;
  }
  [SerializeField] List<PoolData> pools = new();
  readonly Dictionary<string, ObjectPool> poolDictionary = new();
  protected override void OnEnable() {
    base.OnEnable();
    InitializePools();
  }
  void InitializePools() {
    foreach ( var pool in pools.Where(pool => pool.objectPool) ) {
      pool.objectPool.Initialize();
      poolDictionary[pool.id] = pool.objectPool;
    }
  }

  public GameObject Pool(string id) {
    if ( poolDictionary.TryGetValue(id, out var value) ) return value.Pool();
    Debug.LogError($"No pool found with id {id}");
    return null;
  }

  public void Release(string id, GameObject pooledGameObject) {
    if ( !poolDictionary.TryGetValue(id, out var value) ) {
      Debug.LogError($"No pool found with id {id}");
      Destroy(pooledGameObject);
      return;
    }
    value.Release(pooledGameObject);
  }
}
