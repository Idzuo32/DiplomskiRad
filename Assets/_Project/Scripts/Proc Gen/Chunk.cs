using System.Collections.Generic;
using UnityEngine;
using Managers;
using Utilities;

namespace ProceduralGeneration
{
    public enum SpawnType
    {
        Fence,
        Apple,
        Coi1n
    }
    public class Chunk : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField, Tooltip("Fence obstacle prefab (pooled)")] GameObject fencePrefab;
        [SerializeField, Tooltip("Apple pickup prefab (pooled)")] GameObject applePrefab;
        [SerializeField, Tooltip("Coin pickup prefab (pooled)")] GameObject coinPrefab;

        [Header("Spawn Settings")]
        [SerializeField, Tooltip("Chance [0..1] to spawn an apple on this chunk")] float appleSpawnChance = .3f;
        [SerializeField, Tooltip("Chance [0..1] to spawn coins on this chunk")] float coinSpawnChance = .5f;
        [SerializeField, Tooltip("Distance between consecutive coins along Z")] float coinSeparationLength = 2f;

        [SerializeField, Tooltip("World-space X positions for lanes (left to right)")] float[] lanes = { -2.5f, 0f, 2.5f };

        [Header("Runtime Spawn Root")]
        [SerializeField, Tooltip("Parent transform for runtime-spawned objects. If not assigned, a child named 'SpawnRoot' will be created at runtime.")]
        Transform spawnRoot;

        readonly List<int> _availableLanes = new() { 0, 1, 2 };

        void Awake()
        {
            // Ensure we have a dedicated container for spawned (pooled) content so we don't affect static children.
            if (spawnRoot == null)
            {
                // Try find an existing child named SpawnRoot first
                for (int i = 0; i < transform.childCount; i++)
                {
                    var child = transform.GetChild(i);
                    if (child != null && child.name == "SpawnRoot")
                    {
                        spawnRoot = child;
                        break;
                    }
                }

                if (spawnRoot == null)
                {
                    var go = new GameObject("SpawnRoot");
                    spawnRoot = go.transform;
                    spawnRoot.SetParent(transform, false);
                }
            }
        }

        void OnEnable()
        {
            // Ensure we don't keep previously spawned (pooled) children when this chunk is reused
            ClearSpawnedChildren();
            ResetAvailableLanes();
            SpawnObjects();
        }

        void OnDisable()
        {
            // Proactively clear children so pooled chunks don't carry objects into next use
            ClearSpawnedChildren();
        }

        void ResetAvailableLanes()
        {
            _availableLanes.Clear();
            _availableLanes.AddRange(new[] { 0, 1, 2 });
        }

        // Deactivate any previously spawned pooled children to avoid overlap on chunk reuse
        void ClearSpawnedChildren()
        {
            if (spawnRoot == null) return; // safety
            for (int i = spawnRoot.childCount - 1; i >= 0; i--)
            {
                var child = spawnRoot.GetChild(i);
                if (child != null && child.gameObject.activeSelf)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        void SpawnObjects()
        {
            // Deterministic order: fences first so they can occupy up to 2 lanes, then items use remaining lanes
            SpawnFences();
            SpawnApple();
            SpawnCoins();
        }
        void SpawnFences()
        {
            // Allow 0..2 lanes for fences (but never exceed available lanes)
            int maxFenceLanes = Mathf.Min(2, _availableLanes.Count);
            int fencesToSpawn = Random.Range(0, maxFenceLanes + 1); // inclusive upper bound via +1

            for (var i = 0; i < fencesToSpawn; i++)
            {
                if (_availableLanes.Count <= 0) break;

                var selectedLane = SelectLane();

                var spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
                PoolManager.Get(fencePrefab, spawnPosition, Quaternion.identity, spawnRoot);
            }
        }

        void SpawnApple()
        {
            if (Random.value > appleSpawnChance || _availableLanes.Count <= 0) return;

            var selectedLane = SelectLane();

            var spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            PoolManager.Get(applePrefab, spawnPosition, Quaternion.identity, spawnRoot);
        }

        void SpawnCoins()
        {
            if (Random.value > coinSpawnChance || _availableLanes.Count <= 0) return;

            var selectedLane = SelectLane();

            var maxCoinsToSpawn = 6;
            var coinsToSpawn = Random.Range(1, maxCoinsToSpawn);

            var topOfChunkZPos = transform.position.z + (coinSeparationLength * 2f);

            for (var i = 0; i < coinsToSpawn; i++)
            {
                var spawnPositionZ = topOfChunkZPos - (i * coinSeparationLength);
                var spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnPositionZ);
                PoolManager.Get(coinPrefab, spawnPosition, Quaternion.identity, spawnRoot);
            }
        }

        int SelectLane()
        {
            var randomLaneIndex = Random.Range(0, _availableLanes.Count);
            var selectedLane = _availableLanes[randomLaneIndex];
            _availableLanes.RemoveAt(randomLaneIndex);
            return selectedLane;
        }
    }
}