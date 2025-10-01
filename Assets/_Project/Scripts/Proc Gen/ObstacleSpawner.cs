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
        
        bool _gameOver;

        void Start()
        {
            StartCoroutine(SpawnObstacleRoutine());
        }

        public void DecreaseObstacleSpawnTime(float amount)
        {
            obstacleSpawnTime -= amount;

            if (obstacleSpawnTime <= minObstacleSpawnTime) 
            {
                obstacleSpawnTime = minObstacleSpawnTime;
            }
        }

        IEnumerator SpawnObstacleRoutine() 
        {
            while (!_gameOver)
            {
                var obstaclePrefab = obstaclePrefabs[Random.Range(0, obstaclePrefabs.Length)];
                var spawnPosition = new Vector3(Random.Range(-spawnWidth, spawnWidth), transform.position.y, transform.position.z);
                yield return new WaitForSeconds(obstacleSpawnTime);
                var pooledObstacle = PoolManager.Get(obstaclePrefab, spawnPosition, Random.rotation, obstacleParent);
                var obstacleRb = pooledObstacle.GetComponent<Rigidbody>();
                obstacleRb.linearVelocity = Vector3.zero;
                obstacleRb.angularVelocity = Vector3.zero;
            }
        }
    }
}
