using Managers;
using UnityEngine;

namespace ProceduralGeneration
{
    public class Checkpoint : MonoBehaviour
    {
        [SerializeField] float checkpointTimeExtension = 5f;
        [SerializeField] float obstacleDecreaseTimeAmount = .2f;

        GameManager _gameManager;
        ObstacleSpawner _obstacleSpawner;

        const string PlayerString = "Player";

        void Start()
        {
            _gameManager = FindFirstObjectByType<GameManager>();
            _obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PlayerString))
            {
                _gameManager.IncreaseTime(checkpointTimeExtension);
                _obstacleSpawner.DecreaseObstacleSpawnTime(obstacleDecreaseTimeAmount);
            }
        }
    }
}