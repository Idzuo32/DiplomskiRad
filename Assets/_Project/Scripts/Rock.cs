using Managers;
using Unity.Cinemachine;
using UnityEngine;

public class Rock : MonoBehaviour
{   
    [SerializeField] ParticleSystem collisionParticleSystem;
    [SerializeField] AudioClip boulderSmashSound;
    [SerializeField] float shakeModifer = 10f;
    [SerializeField] float collisionCooldown = 1f;

    CinemachineImpulseSource _cinemachineImpulseSource;

    float _collisionTimer = 1f;

    void Awake() 
    {
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    void Update() 
    {
        _collisionTimer += Time.deltaTime;
    }

    void OnCollisionEnter(Collision other)
    {
        if (_collisionTimer < collisionCooldown) return;

        FireImpulse();
        CollisionFX(other);
        _collisionTimer = 0f;
    }

    void FireImpulse()
    {
        if (!Camera.main) return;
        var distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        var shakeIntensity = (1f / distance) * shakeModifer;
        shakeIntensity = Mathf.Min(shakeIntensity, 1f);
        _cinemachineImpulseSource.GenerateImpulse(shakeIntensity);
    }

    void CollisionFX(Collision other)
    {
        var contactPoint  = other.contacts[0];
        collisionParticleSystem.transform.position = contactPoint.point;
        collisionParticleSystem.Play();
        SoundFXManager.Instance.PlaySoundFX(boulderSmashSound, transform, 1f);
    }
}
