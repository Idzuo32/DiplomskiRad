using System.Collections;
using UnityEngine;
using Managers;

namespace ProceduralGeneration
{
    public class ObstacleSpawner : MonoBehaviour
    {
        [SerializeField] GameObject[] obstaclePrefabs;
        [SerializeField] float obstacleSpawnTime = 1f;
        [SerializeField] float minObstacleSpawnTime = .2f;
        [SerializeField] Transform obstacleParent;
        [SerializeField] float spawnWidth = 4f;

        [Header("Difficulty Balance")]
        [SerializeField] float initialDecreaseAmount = 0.1f;
        [SerializeField] float decayRate = 0.8f;
        [SerializeField] float minimumDecreaseAmount = 0.01f;

        private float currentDecreaseAmount;

        bool _gameOver;

        void Start()
        {
            currentDecreaseAmount = initialDecreaseAmount;
            StartCoroutine(SpawnObstacleRoutine());
        }

        public void DecreaseObstacleSpawnTime()
        {
            currentDecreaseAmount = Mathf.Max(
                currentDecreaseAmount * decayRate,
                minimumDecreaseAmount
            );

            obstacleSpawnTime = Mathf.Max(
                obstacleSpawnTime - currentDecreaseAmount,
                minObstacleSpawnTime
            );
        }
        IEnumerator SpawnObstacleRoutine()
        {
            while (!_gameOver)
            {
                var obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                var spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y,
                    transform.position.z);
                yield return new WaitForSeconds(obstacleSpawnTime);
                var pooledObstacle = PoolManager.Get(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);
                var obstacleRb = pooledObstacle.GetComponent<Rigidbody>();
                obstacleRb.linearVelocity = Vector3.zero;
                obstacleRb.angularVelocity = Vector3.zero;
            }
        }
    }
}