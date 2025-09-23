using UnityEngine;

namespace ProceduralGeneration
{
    public class ObstacleDestroy : MonoBehaviour
    {
        void OnTriggerEnter(Collider other) 
        {
            Destroy(other.gameObject);    
        }
    }
}
