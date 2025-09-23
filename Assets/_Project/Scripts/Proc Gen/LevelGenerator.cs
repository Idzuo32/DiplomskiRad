using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;

namespace ProceduralGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        CameraController cameraController;

        [SerializeField] GameObject[] chunkPrefabs;
        [SerializeField] GameObject checkpointChunkPrefab;
        [SerializeField] Transform chunkParent;
        [SerializeField] ScoreManager scoreManager;

        [Header("Level Settings")] [Tooltip("The amount of chunks we start with")] [SerializeField]
        int startingChunksAmount = 12;

        [SerializeField] int checkpointChunkInterval = 8;

        [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")] [SerializeField]
        float chunkLength = 10f;

        [SerializeField] float moveSpeed = 8f;
        [SerializeField] float minMoveSpeed = 2f;
        [SerializeField] float maxMoveSpeed = 20f;
        [SerializeField] float minGravityZ = -22f;
        [SerializeField] float maxGravityZ = -2f;

        Camera _camera;
        readonly List<GameObject> _chunks = new List<GameObject>();
        int _chunksSpawned;

        void Start()
        {
            if (Camera.main)
            {
                _camera = Camera.main;
            }

            SpawnStartingChunks();
        }

        void Update()
        {
            MoveChunks();
        }

        public void ChangeChunkMoveSpeed(float speedAmount)
        {
            var newMoveSpeed = moveSpeed + speedAmount;
            newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

            if (!Mathf.Approximately(newMoveSpeed, moveSpeed))
            {
                moveSpeed = newMoveSpeed;

                var newGravityZ = Physics.gravity.z - speedAmount;
                newGravityZ = Mathf.Clamp(newGravityZ, minGravityZ, maxGravityZ);
                Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

                cameraController.ChangeCameraFOV(speedAmount);
            }
        }

        void SpawnStartingChunks()
        {
            for (var i = 0; i < startingChunksAmount; i++)
            {
                SpawnChunk();
            }
        }

        void SpawnChunk()
        {
            var spawnPositionZ = CalculateSpawnPositionZ();
            var chunkSpawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
            var chunkToSpawn = ChooseChunkToSpawn();
            var newChunkGo = Instantiate(chunkToSpawn, chunkSpawnPos, Quaternion.identity, chunkParent);
            _chunks.Add(newChunkGo);
            var newChunk = newChunkGo.GetComponent<Chunk>();
            newChunk.Init(this, scoreManager);

            _chunksSpawned++;
        }

        GameObject ChooseChunkToSpawn()
        {
            GameObject chunkToSpawn;

            if (_chunksSpawned % checkpointChunkInterval == 0 && _chunksSpawned != 0)
            {
                chunkToSpawn = checkpointChunkPrefab;
            }
            else
            {
                chunkToSpawn = chunkPrefabs[Random.Range(0, chunkPrefabs.Length)];
            }

            return chunkToSpawn;
        }

        float CalculateSpawnPositionZ()
        {
            float spawnPositionZ;

            if (_chunks.Count == 0)
            {
                spawnPositionZ = transform.position.z;
            }
            else
            {
                spawnPositionZ = _chunks[^1].transform.position.z + chunkLength;
            }

            return spawnPositionZ;
        }

        void MoveChunks()
        {
            for (var i = 0; i < _chunks.Count; i++)
            {
                var chunk = _chunks[i];
                chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

                if (!(chunk.transform.position.z <= _camera.transform.position.z - chunkLength)) continue;
                _chunks.Remove(chunk);
                Destroy(chunk);
                SpawnChunk();
            }
        }
    }
}