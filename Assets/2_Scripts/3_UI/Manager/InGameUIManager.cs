using System.Collections;
using System.Collections.Generic;
using JumpRabbit.Core;
using JumpRabbit.UI;
using LSH.Core;
using LSH.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
        #region Fields
        [Header("Score UI")]
        [SerializeField] TextMeshProUGUI _scoreText;
        [SerializeField] TextMeshProUGUI _bonusRateText;

        [Header("Pause Menu")]
        [SerializeField] Button _retryButton;
        [SerializeField] Button _toTitleButton;

        [Header("Popup")]
        [SerializeField] PopupText _popupTextPrefab;
        [SerializeField] float _popupInterval = 0.3f;

        private PopupTextPool _popupPool;
        private ScoreManager _scoreManager;
        private Transform _player;
        #endregion

        #region Init

        public void Init(ScoreManager scoreManager, Transform player)
        {
            _scoreManager = scoreManager;
            _player = player;

            InitPopupPool();
            InitScoreUI();
            SubscribeEvents();
            HidePauseMenu();
        }

        private void InitPopupPool()
        {
            _popupPool = new PopupTextPool(_popupTextPrefab, transform);
            _popupPool.Init();
        }

        private void InitScoreUI()
        {
            UpdateScoreUI(0, 1f);
        }

        private void SubscribeEvents()
        {
            if (_scoreManager == null)
                return;

            _scoreManager.OnScoreChanged -= HandleScoreChanged;
            _scoreManager.OnScoreChanged += HandleScoreChanged;

            _scoreManager.OnScoreBonusRateChanged -= HandleScoreBonusRateChanged;
            _scoreManager.OnScoreBonusRateChanged += HandleScoreBonusRateChanged;
        }

        #endregion

        #region Score UI
        private void HandleScoreChanged(ScoreContext context)
        {
            UpdateScoreUI(context.TotalScore, context.ScoreBonusRate);

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

        private void HandleScoreBonusRateChanged(float bonusRate)
        {
            UpdateBonusRateUI(bonusRate);

            PopupText popupText = _popupPool.Get();
            popupText.transform.position = _player.position;

            popupText.Activate("Bonus Up!", _popupPool.Release);
        }

        private void UpdateScoreUI(int totalScore, float bonusRate)
        {
            _scoreText.text = totalScore.ToString();
            UpdateBonusRateUI(bonusRate);
        }

        private void UpdateBonusRateUI(float bonusRate)
        {
            float bonusPercent = bonusRate * 100f;
            _bonusRateText.text = bonusPercent.ToFormatString() + "%";
        }

        #endregion

        #region Popup
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

        

        private string ScoreToString(int score)
        {
            string text = $"+{score}";
            return text;
        }

        #endregion

        #region Button Events
        public void HandleRetryBtnClicked()
        {
            TransitionManager.Instance.LoadNextScene(SceneName.InGame);
        }

        public void HandleTitleBtnClicked()
        {
            TransitionManager.Instance.LoadNextScene(SceneName.Title);
        }
        #endregion

        #region Pause Menu
        public void ShowPauseMenu()
        {
            _retryButton.gameObject.SetActive(true);
            _toTitleButton.gameObject.SetActive(true);
        }

        public void HidePauseMenu()
        {
            _retryButton?.gameObject.SetActive(false);
            _toTitleButton?.gameObject.SetActive(false);
        }
        #endregion

        #region CleanUp
        private void OnDestroy()
        {
            _scoreManager.OnScoreChanged -= HandleScoreChanged;
            _scoreManager.OnScoreBonusRateChanged -= HandleScoreBonusRateChanged;
        }
        #endregion
    }

}
