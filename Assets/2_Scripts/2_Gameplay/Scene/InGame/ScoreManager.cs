using System;
using JumpRabbit.GamePlay.Entities;
using UnityEngine;

namespace JumpRabbit.GamePlay.InGame
{
    public readonly struct ScoreContext
    {
        public readonly int BaseScore;
        public readonly int AddedScore;
        public readonly int TotalScore;
        public readonly int Combo;
        public readonly float ScoreBonusRate;

        public ScoreContext(
            int baseScore,
            int addedScore,
            int totalScore,
            int combo,
            float scoreBonusRate)
        {
            BaseScore = baseScore;
            AddedScore = addedScore;
            TotalScore = totalScore;
            Combo = combo;
            ScoreBonusRate = scoreBonusRate;
        }
    }

    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] ScoreTable _settings;

        private PlatformManager _platformManager;
        private int _totalScore = 0;

        private int _currentCombo = 0;
        private int _maxCombo = 0;

        private float _currentScoreBonusRate = 1.0f;

        public int Score => _totalScore;
        public int CurrentCombo => _currentCombo;
        public int MaxCombo => Mathf.Max(_maxCombo, _currentCombo);
        public float CurrentScoreBonusRate => _currentScoreBonusRate;

        public event Action<ScoreContext> OnScoreChanged;
        public event Action<float> OnScoreBonusRateChanged;

        public void Init(PlatformManager platformManager)
        {
            _platformManager = platformManager;

            _platformManager.OnValidPlatformLanded += HandleValidPlatformLanded;
            _platformManager.OnInvalidPlatformLanded += HandleInvalidPlatformLanded;
            _platformManager.OnCarrotCollected += HandleCarrotCollected;
        }

        private void HandleValidPlatformLanded(Platform platform)
        {
            _currentCombo++;

            AddScore(platform);
        }

        private void AddScore(Platform platform)
        {
            _settings.TryGetScoreRule(platform.Size, out ScoreRule table);

            _currentScoreBonusRate += table.AdditionalScoreBonusRate;
            float addScore = table.BaseScore * _currentScoreBonusRate;
            _totalScore += (int)addScore;

            ScoreContext context = new ScoreContext(table.BaseScore, (int)addScore, _totalScore, _currentCombo, _currentScoreBonusRate);
            OnScoreChanged?.Invoke(context);
        }

        private void HandleInvalidPlatformLanded(Platform platform)
        {
            _maxCombo = Mathf.Max(_maxCombo, _currentCombo);
            _currentCombo = 0;

            _currentScoreBonusRate = 1.0f;

            ScoreContext context = new ScoreContext(0, 0, _totalScore, _currentCombo, _currentScoreBonusRate);
            OnScoreChanged?.Invoke(context);
        }

        private void HandleCarrotCollected(float additionalBonusRate)
        {
            _currentScoreBonusRate += additionalBonusRate;

            
            OnScoreBonusRateChanged?.Invoke(_currentScoreBonusRate);

        }
    }
}
