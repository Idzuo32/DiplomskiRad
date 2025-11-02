using Managers;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] AudioClip obstacleSound;
    [SerializeField] float collisionCooldown = 1f;
    bool collided;
    float collisionTimer = 1f;

    void Update()
    {
        if (collided) return;
        collisionTimer += Time.deltaTime;
    }

    void OnCollisionEnter()
    {
        if (collisionTimer < collisionCooldown) return;
        collided = true;
        SoundFXManager.Instance.PlaySoundFX(obstacleSound, transform, 1f);
    }

    void OnCollisionExit()
    {
        if (collisionTimer < collisionCooldown) return;
        collided = false;
    }
}