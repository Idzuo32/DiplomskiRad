using UnityEngine;

namespace Pickups
{
    public abstract class Pickup : MonoBehaviour
    {
        [SerializeField] float rotationSpeed = 100f;

        const string PlayerString = "Player";

        void Update() 
        {
            transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }

        void OnTriggerEnter(Collider other) 
        {
            if (other.CompareTag(PlayerString)) 
            {
                OnPickup();   
                Destroy(gameObject);
            }
        }

        protected abstract void OnPickup();
    }
}
