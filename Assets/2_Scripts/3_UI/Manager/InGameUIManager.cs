using System.Collections;
using System.Collections.Generic;
using JumpRabbit.UI;
using LSH.Utils;
using TMPro;
using UnityEngine;

namespace JumpRabbit.GamePlay.InGame
{
    public enum PopupTextType
    {
        Score,
        BonusInfo
    }

    public readonly struct PopupTextData
    {
        public readonly string Text;
        public readonly PopupTextType Type;
        public readonly Vector3 Offset;

        public PopupTextData(string text, PopupTextType type, Vector3 offset)
        {
            Text = text;
            Type = type;
            Offset = offset;
        }
    }

    public class InGameUIManager : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _bonusRateText;

        [Header("Popup")]
        [SerializeField] PopupText _popupTextPrefab;
        [SerializeField] float _popupInterval = 0.3f;

        private PopupTextPool _popupPool;
        private ScoreManager _scoreManager;
        private Transform _player;

        public void Init(ScoreManager scoreManager, Transform player)
        {
            _popupPool = new PopupTextPool(_popupTextPrefab, this.transform);
            _popupPool.Init();
            
            _scoreText.text = "0";
            _bonusRateText.text = "100%";

            _scoreManager = scoreManager;
            _player = player;

            _scoreManager.OnScoreChanged += HandleScoreChanged;
            _scoreManager.OnScoreBonusRateChanged += HandleScoreBonusRateChanged;
        }

        private void HandleScoreChanged(ScoreContext context)
        {
            _scoreText.text = context.TotalScore.ToString();
            _bonusRateText.text = (context.ScoreBonusRate * 100).ToFormatString() + "%";

            if(context.AddedScore > 0)
            {
                ShowScorePopup(context, _player.position);
            }
            else
            {
                PopupText popupText = _popupPool.Get();
                popupText.transform.position = _player.position;

                popupText.Activate("보너스 초기화...", _popupPool.Release);
            }
        }

        private void ShowScorePopup(ScoreContext context, Vector3 position)
        {
            StartCoroutine(ShowScorePopupSequence(context, position));
        }

        private IEnumerator ShowScorePopupSequence(ScoreContext context, Vector3 position)
        {
            List<PopupTextData> popupTexts = CreatePopupTexts(context);


            for (int i = 0; i < popupTexts.Count; i++)
            {
                ShowPopup(popupTexts[i], position);

                yield return new WaitForSeconds(_popupInterval);
            }
            
        }

        private List<PopupTextData> CreatePopupTexts(ScoreContext context)
        {
            List<PopupTextData> result = new List<PopupTextData>()
            {
                new PopupTextData(
                    ScoreToString(context.BaseScore),
                    PopupTextType.Score,
                    new Vector3(1,0,0)),

                new PopupTextData(
                    $"+{Mathf.RoundToInt((context.ScoreBonusRate - 1) * 100)}%",
                    PopupTextType.BonusInfo,
                    new Vector3(0,0,0)),

                new PopupTextData(
                    ScoreToString(context.AddedScore - context.BaseScore),
                    PopupTextType.Score,
                    new Vector3(-1,0,0))

            };

            return result;
        }
        private void ShowPopup(PopupTextData textData, Vector3 position)
        {
            PopupText popupText = _popupPool.Get();

            popupText.transform.position = position + textData.Offset;
            popupText.Activate(textData, _popupPool.Release);
        }

        private void HandleScoreBonusRateChanged(float bonusRate)
        {
            _bonusRateText.text = (bonusRate * 100).ToFormatString() + "%";

            PopupText popupText = _popupPool.Get();
            popupText.transform.position = _player.position;

            popupText.Activate("Bonus Up!", _popupPool.Release);
        }

        private string ScoreToString(int score)
        {
            string text = $"+{score}";
            return text;
        }

        
    }

}
