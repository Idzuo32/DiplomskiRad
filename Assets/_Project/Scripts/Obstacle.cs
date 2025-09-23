using Managers;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] AudioClip obstacleSound;
    [SerializeField] float collisionCooldown = 1f;
    bool _collided;
    float _collisionTimer = 1f;

    void Update()
    {
        if (_collided) return;
        _collisionTimer += Time.deltaTime;
    }

    void OnCollisionEnter()
    {
        if (_collisionTimer < collisionCooldown) return;
        _collided = true;
        SoundFXManager.Instance.PlaySoundFX(obstacleSound, transform, 1f);
    }

    void OnCollisionExit()
    {
        if (_collisionTimer < collisionCooldown) return;
        _collided = false;
    }
}