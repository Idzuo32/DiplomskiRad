using System;
using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public static event Action OnGameStart;
        public static event Action OnGameOver;
        [SerializeField] TMP_Text timeText;

        [SerializeField] AudioClip gameOverSound;
        [SerializeField] AudioClip gameWonSound;

        [SerializeField] TMP_Text highScoreText;

        [SerializeField] TMP_Text gameOverTitle;

        [SerializeField] float startTime = 5f;
        [SerializeField] AudioClip timeIncreasedSound;


        float _timeLeft;

        public bool GameOver { get; private set; }

        void Start()
        {
            SaveSystem.LoadGame();
            StartLevel();
        }

        void StartLevel()
        {
            GameOver = false;
            startTime = 30f;
            UIManager.Instance.ShowPanel("GamePanel");
            _timeLeft = startTime;
            OnGameStart?.Invoke();
        }

        void Update()
        {
            DecreaseTime();
        }

        public void IncreaseTime(float amount)
        {
            _timeLeft += amount;
            SoundFXManager.Instance.PlaySoundFX(timeIncreasedSound, transform, 1f);
        }

        void DecreaseTime()
        {
            if (GameOver) return;

            _timeLeft -= Time.deltaTime;
            timeText.text = _timeLeft.ToString("F1");

            if (_timeLeft <= 0f)
            {
                PlayerGameOver();
            }
        }

        void PlayerGameOver()
        {
            GameOver = true;
            SaveSystem.SaveGame();
            UIManager.Instance.ShowPanel("GameOverPanel");
            Time.timeScale = 0;
            OnGameOver?.Invoke();
            if (ScoreManager.Instance.score > ScoreManager.Instance.highScore)
            {
                highScoreText.text = ScoreManager.Instance.score.ToString();
                gameOverTitle.text = "Well Done!";
                SoundFXManager.Instance.PlaySoundFX(gameWonSound, transform, 1f);
            }
            else
            {
                highScoreText.text = ScoreManager.Instance.highScore.ToString();
                gameOverTitle.text = "Too Bad!";
                SoundFXManager.Instance.PlaySoundFX(gameOverSound, transform, 1f);
            }
        }
    }
}