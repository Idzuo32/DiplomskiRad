using UnityEngine;
using Managers;

namespace ProceduralGeneration
{
    public class ObstacleDestroy : MonoBehaviour
    {
        void OnTriggerEnter(Collider other) 
        {
            PoolManager.Release(other.gameObject);
        }
    }
}
