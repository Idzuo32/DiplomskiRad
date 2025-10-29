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
                if (other.CompareTag(PlayerString))
                {
                    if (GameManager.HasInstance)
                    {
                        GameManager.Instance.IncreaseTime(checkpointTimeExtension);
                    }

                    if (_obstacleSpawner)
                    {
                        _obstacleSpawner.DecreaseObstacleSpawnTime();
                    }
                }
            }
        }
    }
}
