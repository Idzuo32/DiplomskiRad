using TMPro;
using UnityEngine;
using Utilities;

namespace Managers
{
    public class ScoreManager : Singleton<ScoreManager>
    {

        [SerializeField] TMP_Text scoreText;

        [SerializeField] TMP_Text finalScoreText;
        public int score;
        public int highScore;

        void Start()
        {
            score = 0;
            SaveSystem.LoadGame();
            finalScoreText.text = highScore.ToString();
        }

        public void IncreaseScore(int amount)
        {
            if (GameManager.HasInstance && GameManager.Instance.GameOver) return;

            score += amount;
            scoreText.text = score.ToString();
            finalScoreText.text = scoreText.text;
        }

        public void Save(ref ScoreData scoreData)
        {
            if (score > highScore)
            {
                scoreData.highScore = score;
            }
        }

        public void Load(ScoreData scoreData)
        {
            highScore = scoreData.highScore;
        }
    }

    [System.Serializable]
    public struct ScoreData
    {
        public int highScore;
    }
}