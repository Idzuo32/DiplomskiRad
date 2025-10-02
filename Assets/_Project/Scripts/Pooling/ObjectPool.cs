using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class ObjectPool
    {
        readonly GameObject prefab;
        readonly Transform poolRoot;
        readonly Queue<GameObject> items = new();

        public ObjectPool(GameObject prefab, Transform poolRoot, int prewarm = 0)
        {
            this.prefab = prefab;
            this.poolRoot = poolRoot;

            for (var i = 0; i < prewarm; i++)
            {
                var go = CreateInstance();
                Return(go);
            }
        }

        GameObject CreateInstance()
        {
            var go = Object.Instantiate(prefab, poolRoot);
            go.SetActive(false);
            var po = go.GetComponent<PooledObject>();
            if (po == null) po = go.AddComponent<PooledObject>();
            po.PrefabKey = prefab;
            return go;
        }

        public GameObject Get(Vector3 position, Quaternion rotation, Transform parent)
        {
            var go = items.Count > 0 ? items.Dequeue() : CreateInstance();
            go.transform.SetParent(parent, false);
            go.transform.SetPositionAndRotation(position, rotation);
            go.SetActive(true);
            return go;
        }

        public void Return(GameObject go)
        {
            if (go == null) return;
            go.SetActive(false);
            go.transform.SetParent(poolRoot, false);
            items.Enqueue(go);
        }
    }
}