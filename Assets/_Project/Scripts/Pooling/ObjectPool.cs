using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public class ObjectPool
    {
        readonly GameObject _prefab;
        readonly Transform _poolRoot;
        readonly Queue<GameObject> _items = new Queue<GameObject>();

        public ObjectPool(GameObject prefab, Transform poolRoot, int prewarm = 0)
        {
            _prefab = prefab;
            _poolRoot = poolRoot;

            for (var i = 0; i < prewarm; i++)
            {
                var go = CreateInstance();
                Return(go);
            }
        }

        GameObject CreateInstance()
        {
            var go = Object.Instantiate(_prefab, _poolRoot);
            go.SetActive(false);
            var po = go.GetComponent<PooledObject>();
            if (po == null) po = go.AddComponent<PooledObject>();
            po.PrefabKey = _prefab;
            return go;
        }

        public GameObject Get(Vector3 position, Quaternion rotation, Transform parent)
        {
            var go = _items.Count > 0 ? _items.Dequeue() : CreateInstance();
            go.transform.SetParent(parent, false);
            go.transform.SetPositionAndRotation(position, rotation);
            go.SetActive(true);
            return go;
        }

        public void Return(GameObject go)
        {
            if (go == null) return;
            go.SetActive(false);
            go.transform.SetParent(_poolRoot, false);
            _items.Enqueue(go);
        }
    }
}