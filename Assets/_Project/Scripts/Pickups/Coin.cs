using Managers;
using UnityEngine;

namespace Pickups
{
    public class Coin : Pickup
    {
        [SerializeField] int scoreAmount = 100;
        [SerializeField] AudioClip coinSound;

        protected override void OnPickup()
        {
            ScoreManager.Instance.IncreaseScore(scoreAmount);
            SoundFXManager.Instance.PlaySoundFX(coinSound, transform, 1f);
        }
    }
}
