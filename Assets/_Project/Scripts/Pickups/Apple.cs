using Managers;
using ProceduralGeneration;
using UnityEngine;

namespace Pickups
{
    public class Apple : Pickup
    {
        [SerializeField] float adjustChangeMoveSpeedAmount = 3f;
        [SerializeField] AudioClip appleSound;
        LevelGenerator _levelGenerator;

        public void Init(LevelGenerator levelGenerator) 
        {
            this._levelGenerator = levelGenerator;
        }

        protected override void OnPickup()
        {
            _levelGenerator.ChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
            SoundFXManager.Instance.PlaySoundFX(appleSound, transform, 1f);
        }
    }
}
