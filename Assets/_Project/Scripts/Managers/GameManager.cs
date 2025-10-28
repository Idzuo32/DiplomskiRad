using System;
using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class GameManager : SceneSingleton<GameManager>
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

        void Awake()
        {
            // Validate required serialized references at scene load.
            if (!timeText) Debug.LogError("[GameManager] 'timeText' is not assigned.", this);
            if (!highScoreText) Debug.LogError("[GameManager] 'highScoreText' is not assigned.", this);
            if (!gameOverTitle) Debug.LogError("[GameManager] 'gameOverTitle' is not assigned.", this);
            if (!timeIncreasedSound) Debug.LogWarning("[GameManager] 'timeIncreasedSound' is not assigned.", this);
            if (!gameOverSound) Debug.LogWarning("[GameManager] 'gameOverSound' is not assigned.", this);
            if (!gameWonSound) Debug.LogWarning("[GameManager] 'gameWonSound' is not assigned.", this);
        }

        void OnDisable()
        {
            // Clear static events to avoid dangling listeners across scene reloads
            OnGameStart = null;
            OnGameOver = null;
        }

        void Start()
        {
            SaveSystem.LoadGame();
            StartLevel();
        }

        void StartLevel()
        {
            GameOver = false;
            startTime = 30f;
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ShowPanel("GamePanel");
            }
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
            if (SoundFXManager.HasInstance && timeIncreasedSound)
            {
                SoundFXManager.Instance.PlaySoundFX(timeIncreasedSound, transform, 1f);
            }
        }

        void DecreaseTime()
        {
            if (GameOver) return;

            _timeLeft -= Time.deltaTime;
            if (timeText)
            {
                timeText.text = _timeLeft.ToString("F1");
            }

            if (_timeLeft <= 0f)
            {
                PlayerGameOver();
            }
        }

        void PlayerGameOver()
        {
            GameOver = true;
            SaveSystem.SaveGame();
            if (UIManager.HasInstance)
            {
                UIManager.Instance.ShowPanel("GameOverPanel");
            }
            Time.timeScale = 0;
            OnGameOver?.Invoke();
            if (ScoreManager.Instance.score > ScoreManager.Instance.highScore)
            {
                if (highScoreText) highScoreText.text = ScoreManager.Instance.score.ToString();
                if (gameOverTitle) gameOverTitle.text = "Well Done!";
                if (SoundFXManager.HasInstance && gameWonSound)
                    SoundFXManager.Instance.PlaySoundFX(gameWonSound, transform, 1f);
            }
            else
            {
                if (highScoreText) highScoreText.text = ScoreManager.Instance.highScore.ToString();
                if (gameOverTitle) gameOverTitle.text = "Too Bad!";
                if (SoundFXManager.HasInstance && gameOverSound)
                    SoundFXManager.Instance.PlaySoundFX(gameOverSound, transform, 1f);
            }
        }
    }
}