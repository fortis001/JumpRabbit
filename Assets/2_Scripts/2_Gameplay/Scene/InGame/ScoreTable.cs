using System;
using System.Collections.Generic;
using JumpRabbit.GamePlay.Entities;
using UnityEngine;


namespace JumpRabbit.GamePlay.InGame
{
    [Serializable]
    public struct ScoreRule
    {
        [SerializeField] private PlatformSize _size;
        [SerializeField] private int _baseScore;
        [SerializeField] private float _additionalScoreBonusRate;

        public PlatformSize Size => _size;
        public int BaseScore => _baseScore;
        public float AdditionalScoreBonusRate => _additionalScoreBonusRate;
    }
    
    [CreateAssetMenu(
            fileName = "ScoreSettings",
            menuName = "JumpRabbit/Gameplay/Score Settings")]
    public class ScoreTable : ScriptableObject
    {
        [SerializeField] private List<ScoreRule> _scoreSettings = new();

        public List<ScoreRule> ScoreSetting => _scoreSettings;

        public bool TryGetScoreRule(PlatformSize size, out ScoreRule result)
        {
            foreach (ScoreRule setting in _scoreSettings)
            {
                if (setting.Size == size)
                {
                    result = setting;
                    return true;
                }
            }

            result = default;
            return false;
        }
    }
}
