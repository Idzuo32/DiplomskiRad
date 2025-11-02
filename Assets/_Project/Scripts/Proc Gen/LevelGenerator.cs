using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProceduralGeneration
{
    public class LevelGenerator : MonoBehaviour
    {
        public static event Action<float, float> OnChangeChunkMoveSpeed;

        [Header("References")] [SerializeField]
        CameraController cameraController;

        [SerializeField] GameObject[] chunkPrefabs;
        [SerializeField] GameObject checkpointChunkPrefab;
        [SerializeField] Transform chunkParent;

        [Header("Level Settings")] [Tooltip("The amount of chunks we start with")] [SerializeField]
        int startingChunksAmount = 12;

        [SerializeField] int checkpointChunkInterval = 8;

        [Tooltip("Do not change chunk length value unless chunk prefab size reflects change")] [SerializeField]
        float chunkLength = 10f;

        [SerializeField] float moveSpeed = 10f;
        [SerializeField] float minMoveSpeed = 4f;
        [SerializeField] float maxMoveSpeed = 16f;
        [SerializeField] float minGravityZ = -22f;
        [SerializeField] float maxGravityZ = -6f;
        
        public float MoveSpeed => moveSpeed;
        public float MinMoveSpeed => minMoveSpeed;
        public float MaxMoveSpeed => maxMoveSpeed;
        
        Camera cam;
        readonly List<GameObject> chunks = new List<GameObject>();
        int chunksSpawned;

        void OnEnable()
        {
            OnChangeChunkMoveSpeed += ChangeChunkMoveSpeed;
        }

        void OnDisable()
        {
            OnChangeChunkMoveSpeed -= ChangeChunkMoveSpeed;
        }

        void Start()
        {
            if (Camera.main)
            {
                cam = Camera.main;
            }

            SpawnStartingChunks();
        }

        void Update()
        {
            MoveChunks();
        }
        
        public static void HangleChangeChunkMoveSpeed(float speedAmount, float duration)
        {
            OnChangeChunkMoveSpeed?.Invoke(speedAmount, duration);
        }

        void ChangeChunkMoveSpeed(float speedAmount, float duration)
        {
            var applied = ApplySpeedDelta(speedAmount);

            if (duration > 0f && !Mathf.Approximately(applied, 0f))
            {
                StartCoroutine(GraduallyRevertOverTime(duration, applied));
            }
        }

        float ApplySpeedDelta(float delta)
        {
            var target = Mathf.Clamp(moveSpeed + delta, minMoveSpeed, maxMoveSpeed);
            var applied = target - moveSpeed;
            if (Mathf.Approximately(applied, 0f)) return 0f;

            moveSpeed = target;

            var newGravityZ = Mathf.Clamp(Physics.gravity.z - applied, minGravityZ, maxGravityZ);
            Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, newGravityZ);

            if (cameraController)
            {
                cameraController.ChangeCameraFOV(applied);
            }

            return applied;
        }

        IEnumerator GraduallyRevertOverTime(float duration, float totalApplied)
        {
            
            float remaining = totalApplied;
            float elapsed = 0f;

            while (elapsed < duration && !Mathf.Approximately(remaining, 0f))
            {
                float t = Mathf.Clamp01(elapsed / duration);
                float targetRemaining = Mathf.Lerp(totalApplied, 0f, t);
                float desiredDelta = targetRemaining - remaining;

                if (!Mathf.Approximately(desiredDelta, 0f))
                {
                    float actuallyApplied = ApplySpeedDelta(desiredDelta);
                    remaining += actuallyApplied;
                }

                elapsed += Time.deltaTime;
                yield return null;
            }

            // Apply any tiny remainder to fully return to baseline
            if (!Mathf.Approximately(remaining, 0f))
            {
                ApplySpeedDelta(-remaining);
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
            var newChunkGo = PoolManager.Get(chunkToSpawn, chunkSpawnPos, Quaternion.identity, chunkParent);
            chunks.Add(newChunkGo);
            var newChunk = newChunkGo.GetComponent<Chunk>();
            chunksSpawned++;
        }

        GameObject ChooseChunkToSpawn()
        {
            GameObject chunkToSpawn;

            if (chunksSpawned % checkpointChunkInterval == 0 && chunksSpawned != 0)
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

            if (chunks.Count == 0)
            {
                spawnPositionZ = transform.position.z;
            }
            else
            {
                spawnPositionZ = chunks[^1].transform.position.z + chunkLength;
            }

            return spawnPositionZ;
        }

        void MoveChunks()
        {
            for (var i = 0; i < chunks.Count; i++)
            {
                var chunk = chunks[i];
                chunk.transform.Translate(-transform.forward * (moveSpeed * Time.deltaTime));

                if (!(chunk.transform.position.z <= cam.transform.position.z - chunkLength)) continue;
                chunks.Remove(chunk);
                PoolManager.Release(chunk);
                SpawnChunk();
            }
        }
    }
}