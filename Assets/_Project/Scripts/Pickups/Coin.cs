using Managers;
using UnityEngine;

namespace Pickups
{
    public class Coin : Pickup
    {
        [SerializeField] int scoreAmount = 100;
        [SerializeField] AudioClip coinSound;

        ScoreManager _scoreManager;

        public void Init(ScoreManager scoreManager) 
        {
            this._scoreManager = scoreManager;
        }

        protected override void OnPickup()
        {
            _scoreManager.IncreaseScore(scoreAmount);
            SoundFXManager.Instance.PlaySoundFX(coinSound, transform, 1f);
        }
    }
}
