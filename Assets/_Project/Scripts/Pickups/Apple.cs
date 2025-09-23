using Managers;
using ProceduralGeneration;
using UnityEngine;

namespace Pickups
{
    public class Apple : Pickup
    {
        [SerializeField] float adjustChangeMoveSpeedAmount = 3f;
        [SerializeField] AudioClip appleSound;
   

        protected override void OnPickup()
        {
            LevelGenerator.HangleChangeChunkMoveSpeed(adjustChangeMoveSpeedAmount);
            SoundFXManager.Instance.PlaySoundFX(appleSound, transform, 1f);
        }
    }
}
