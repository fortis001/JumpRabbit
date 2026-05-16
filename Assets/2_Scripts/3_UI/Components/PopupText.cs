using System;
using System.Collections;
using JumpRabbit.GamePlay.InGame;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

namespace JumpRabbit.UI
{
    public class PopupText : MonoBehaviour
    {
        [SerializeField] TextMeshPro _text;
        [SerializeField] private Animation _animation;
        [SerializeField] private AnimationClip _showClip;

        [SerializeField] private Color _scoreColor = Color.white;
        [SerializeField] private Color _bonusInfoColor = Color.yellow;

        private Coroutine _playCoroutine;
        private Action<PopupText> _onFinished;


        public void Activate(PopupTextData textData,  Action<PopupText> onFinished)
        {
            _text.color = GetColor(textData.Type);
            Activate(textData.Text, onFinished);
        }
        public void Activate(int score, Action<PopupText> onFinished)
        {
            Activate($"+{score}", onFinished);
        }

        public void Activate(string text, Action<PopupText> OnFinished)
        {
            _text.text = text;
            _onFinished = OnFinished;
            gameObject.SetActive(true);

            if (_playCoroutine != null)
            {
                StopCoroutine(_playCoroutine);
            }

            _playCoroutine = StartCoroutine(PlaySequence());
        }
        private Color GetColor(PopupTextType type)
        {
            return type switch
            {
                PopupTextType.Score => _scoreColor,
                PopupTextType.BonusInfo => _bonusInfoColor,
                _ => _scoreColor
            };
        }

        private IEnumerator PlaySequence()
        {
            if (_animation != null && _showClip != null)
            {
                _animation.clip = _showClip;
                _animation.Play();
                yield return new WaitForSeconds(_showClip.length);
            }

            _playCoroutine = null;
            Resetcolor();
            _onFinished?.Invoke(this);
        }

        private void Resetcolor()
        {
            _text.color = Color.white;
        }
    }
}
