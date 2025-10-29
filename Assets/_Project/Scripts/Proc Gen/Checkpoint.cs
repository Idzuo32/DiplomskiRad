using Managers;
using UnityEngine;

namespace ProceduralGeneration
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] float checkpointTimeExtension = 5f;


        ObstacleSpawner _obstacleSpawner;

        const string PlayerString = "Player";

        void Start()
        {
            _obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerString))
            {
                GameManager.Instance.IncreaseTime(checkpointTimeExtension);
                _obstacleSpawner.DecreaseObstacleSpawnTime();
            }
        }
    }
}
