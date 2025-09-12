using System.Collections.Generic;
using UnityEngine;
using Utilities;


public class PoolProvider : Singleton<PoolProvider> {
    
    [System.Serializable]
    public class PoolEntry {
        public string id;
        public GameObject prefab;
        public int initialSize = 10;
    }


    [SerializeField] List<PoolEntry> pools = new();


    readonly Dictionary<string, ObjectPool> poolDictionary = new();


    protected override void Awake() {

        base.Awake();
        foreach (var entry in pools) {
            var obj = new GameObject($"Pool_{entry.id}");
            obj.transform.SetParent(transform);
            var pool = obj.AddComponent<ObjectPool>();
            pool.GetType().GetField("prefab", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pool, entry.prefab);
            pool.GetType().GetField("initialSize", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(pool, entry.initialSize);
            poolDictionary[entry.id] = pool;
        }
    }
  
    public GameObject Get(string id) {
      if (poolDictionary.TryGetValue(id, out var value)) return value.Get();
      Debug.LogError($"No pool found with id {id}");
        return null;
    }

    public void Release(string id, GameObject obj) {
        if (!poolDictionary.TryGetValue(id, out var value)) {
            Debug.LogError($"No pool found with id {id}");
            Destroy(obj);
            return;
        }
        value.Release(obj);
    }
}