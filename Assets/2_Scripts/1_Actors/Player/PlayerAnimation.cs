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

    public class PlayerAnimation : MonoBehaviour, IBootable
    {
        [SerializeField] private Animator _animator;

        private static readonly int StateID = Animator.StringToHash("StateID");

        public void Init()
        {
            SetState(PlayerAnimState.Idle);
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