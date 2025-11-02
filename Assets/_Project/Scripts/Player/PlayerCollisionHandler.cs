using Managers;
using ProceduralGeneration;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        static readonly int Hit = Animator.StringToHash(HitString);
        [SerializeField] Animator animator;
        [SerializeField] float collisionCooldown = 1f;
        [SerializeField] float adjustChangeMoveSpeedAmount = -2f;
        [SerializeField, Tooltip("If > 0, slow is temporary and reverts after this duration (seconds). If <= 0, change is permanent.")] 
        float speedChangeDuration;
        [SerializeField] AudioClip obstacleHitSound;

        const string HitString = "Hit";
        float cooldownTimer = 1f;


        void Update()
        {
            cooldownTimer += Time.deltaTime;
        }

        void OnCollisionEnter()
        {
            if (cooldownTimer < collisionCooldown) return;

            LevelGenerator.HangleChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount, speedChangeDuration);
            animator.SetTrigger(Hit);
            cooldownTimer = 0f;
            SoundFXManager.Instance.PlaySoundFX(obstacleHitSound, transform, 1f);
        }
    }
}