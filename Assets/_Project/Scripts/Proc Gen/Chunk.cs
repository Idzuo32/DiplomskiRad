using System.Collections.Generic;
using UnityEngine;
using Managers;

namespace ProceduralGeneration
{
    public class Chunk : MonoBehaviour
    {
        [SerializeField] GameObject fencePrefab;
        [SerializeField] GameObject applePrefab;
        [SerializeField] GameObject coinPrefab;

        [SerializeField] float appleSpawnChance = .3f;
        [SerializeField] float coinSpawnChance = .5f;
        [SerializeField] float coinSeparationLength = 2f;

        [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };

        readonly List<int> _availableLanes = new() { 0, 1, 2 };

        void OnEnable()
        {
            _availableLanes.Clear();
            _availableLanes.AddRange(new[] { 0, 1, 2 });
            SpawnFences();
            SpawnApple();
            SpawnCoins();
        }

        void SpawnFences()
        {
            var fencesToSpawn = Random.Range(0, lanes.Length - 1);

            for (var i = 0; i < fencesToSpawn; i++)
            {
                if (_availableLanes.Count <= 0) break;

                var selectedLane = SelectLane();

                var spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
                PoolManager.Get(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
            }
        }

        void SpawnApple()
        {
            if (Random.value > appleSpawnChance || _availableLanes.Count <= 0) return;

            var selectedLane = SelectLane();

            var spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            PoolManager.Get(applePrefab, spawnPosition, Quaternion.identity, this.transform);
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
                PoolManager.Get(coinPrefab, spawnPosition, Quaternion.identity, transform);
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