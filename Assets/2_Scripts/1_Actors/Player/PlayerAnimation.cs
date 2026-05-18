using System.Collections;
using LSH.Core;
using UnityEngine;

namespace JumpRabbit.Actors.Player
{
    public enum PlayerAnimState
    {
        Idle = 0,
        ReadyJump = 1,
        Jump = 2
    }

    public class PlayerAnimation : MonoBehaviour
    {
        [Header("PlayerAnim")]
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        [Header("JumpEffect")]
        [SerializeField] private Animator _jumpEffect;
        [SerializeField] private float _effectDuration = 0.23f;

        private static readonly int StateID = Animator.StringToHash("StateID");
        private Coroutine _jumpEffectCoroutine;

        public void Init(SpriteRenderer spriteRenderer)
        {
            _spriteRenderer = spriteRenderer;
            SetState(PlayerAnimState.Idle);
        }

        public void LookAt(Vector2 worldTarget)
        {
            float directionX = worldTarget.x - transform.position.x;

            if (Mathf.Abs(directionX) <= 0.01f)
                return;

            _spriteRenderer.flipX = directionX < 0f;
        }




        public void PlayIdle() => SetState(PlayerAnimState.Idle);
        public void PlayReadyJump() => SetState(PlayerAnimState.ReadyJump);


        public void PlayJump()
        {
            SetState(PlayerAnimState.Jump);

            PlayJumpEffect();
        }

        private void PlayJumpEffect()
        {
            if (_jumpEffect == null || _jumpEffect == null)
                return;

            if (_jumpEffectCoroutine != null)
                StopCoroutine(_jumpEffectCoroutine);
            _jumpEffectCoroutine = StartCoroutine(PlayJumpEffectSequence());
        }

        private IEnumerator PlayJumpEffectSequence()
        {
            _jumpEffect.transform.SetParent(null);
            _jumpEffect.gameObject.SetActive(true);

            _jumpEffect.Play(0, 0, 0f);

            yield return new WaitForSeconds(_effectDuration);

            _jumpEffect.gameObject.SetActive(false);
            _jumpEffect.transform.SetParent(transform, false);
            _jumpEffect.transform.localPosition = Vector3.zero;

            _jumpEffectCoroutine = null;
        }


        private void SetState(PlayerAnimState state)
        {
            if (_animator == null)
            {
                Debug.LogError("Animator가 할당되지 않았습니다.", this);
                return;
            }

            _animator.SetInteger(StateID, (int)state);
        }
    }
}