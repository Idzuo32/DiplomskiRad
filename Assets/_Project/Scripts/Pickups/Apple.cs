using Managers;
using ProceduralGeneration;
using UnityEngine;

namespace Pickups
{
    public class Apple : Pickup
    {
        [SerializeField] float adjustChangeMoveSpeedAmount = 3f;
        [SerializeField, Tooltip("If > 0, speed change is temporary and reverts after this duration (seconds). If <= 0, change is permanent.")] 
        float speedChangeDuration;
        [SerializeField] AudioClip appleSound;
        ParticleSystem speedupParticleSystem;
        
        void OnEnable()
        {
            speedupParticleSystem = FindAnyObjectByType<ParticleSystem>();
        }

        protected override void OnPickup()
        {
            LevelGenerator.HangleChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount, speedChangeDuration);
            SoundFXManager.Instance.PlaySoundFX(appleSound, transform, 1f);
            speedupParticleSystem.Play();
        }
    }
}