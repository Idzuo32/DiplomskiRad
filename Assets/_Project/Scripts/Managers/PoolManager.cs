using System.Collections.Generic;
using UnityEngine;
using Pooling;

namespace Managers
{
    public static class PoolManager
    {
        static readonly Dictionary<GameObject, ObjectPool> Pools = new();
        static Transform _root;

        static Transform Root
        {
            get
            {
                if (_root == null)
                {
                    var go = new GameObject("[PoolManager]");
                    Object.DontDestroyOnLoad(go);
                    _root = go.transform;
                }

                return _root;
            }
        }

        static ObjectPool GetOrCreatePool(GameObject prefab)
        {
            if (prefab == null) return null;
            if (!Pools.TryGetValue(prefab, out var pool))
            {
                var poolRoot = new GameObject($"Pool - {prefab.name}").transform;
                poolRoot.SetParent(Root, false);
                pool = new ObjectPool(prefab, poolRoot);
                Pools[prefab] = pool;
            }

            return pool;
        }

        public static GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            var pool = GetOrCreatePool(prefab);
            return pool.Get(position, rotation, parent);
        }

        public static void Release(GameObject go)
        {
            if (go == null) return;

            // Release pooled children first to avoid reactivating stale children with parent reuse
            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                var child = go.transform.GetChild(i).gameObject;
                if (child.TryGetComponent<PooledObject>(out _))
                {
                    Release(child);
                }
            }

            if (go.TryGetComponent<PooledObject>(out var po) && po.PrefabKey != null)
            {
                var pool = GetOrCreatePool(po.PrefabKey);
                pool.Return(go);
            }
            else
            {
                Object.Destroy(go);
            }
        }

        public static void ReleaseIfPooled(GameObject go)
        {
            if (go == null) return;
            if (go.TryGetComponent<PooledObject>(out var po) && po.PrefabKey != null)
            {
                Release(go);
            }
            else
            {
                Object.Destroy(go);
            }
        }
    }
}