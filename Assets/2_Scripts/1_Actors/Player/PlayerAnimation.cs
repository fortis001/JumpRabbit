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
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private static readonly int StateID = Animator.StringToHash("StateID");

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
        public void PlayJump() => SetState(PlayerAnimState.Jump);


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