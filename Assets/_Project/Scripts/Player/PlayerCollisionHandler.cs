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
        [SerializeField] AudioClip obstacleHitSound;


        const string HitString = "Hit";
        float _cooldownTimer;

        LevelGenerator _levelGenerator;

        void Start()
        {
            _levelGenerator = FindFirstObjectByType<LevelGenerator>();
        }

        void Update() 
        {
            _cooldownTimer += Time.deltaTime;
        }

        void OnCollisionEnter() 
        {
            if (_cooldownTimer < collisionCooldown) return;

            _levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
            animator.SetTrigger(Hit);
            _cooldownTimer = 0f;
            SoundFXManager.Instance.PlaySoundFX(obstacleHitSound, transform, 1f);
        }
    }
}
