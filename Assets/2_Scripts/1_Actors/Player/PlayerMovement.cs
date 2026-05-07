using JumpRabbit.Core;
using UnityEngine;


namespace JumpRabbit.Actors.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Rigidbody2D _rigidbody = null;
        [SerializeField] private Transform _groundCheck;

        [Header("Jump")]
        [SerializeField] private float _groundCheckDisableTimeAfterJump = 0.1f;
        [SerializeField, Range(0f, 1f)] private float _landingHorizontalDamping = 0.5f;

        [Header("Ground Check")]
        [SerializeField] private Vector2 _groundCheckSize = new Vector2(0.4f, 0.08f);
        [SerializeField] private LayerMask _groundLayer;


        private float _lastJumpTime = -999f;
        private bool _wasGrounded;
        public bool IsGrounded { get; private set; }

        private bool CanCheckGround =>
            TimeManager.Instance.GameTime.Time >=
            _lastJumpTime + _groundCheckDisableTimeAfterJump;


        private void FixedUpdate()
        {
            _wasGrounded = IsGrounded;

            CheckGround();

            if (!_wasGrounded && IsGrounded && _rigidbody.linearVelocity.y <= 0f)
            {
                OnLanded();
            }
        }

        private void CheckGround()
        {
            if (!CanCheckGround)
            {
                IsGrounded = false;
                return;
            }

            IsGrounded = Physics2D.OverlapBox(
                _groundCheck.position,
                _groundCheckSize,
                0f,
                _groundLayer);
        }

        public bool TryJumpTo(Vector2 worldTarget, float jumpPower)
        {
            if (!IsGrounded)
                return false;

            Vector2 direction = GetDirection(worldTarget);

            _lastJumpTime = TimeManager.Instance.GameTime.Time;

            IsGrounded = false;
            _rigidbody.linearVelocity = direction * jumpPower;

            return true;
        }

        private Vector2 GetDirection(Vector2 worldTarget)
        {
            Vector2 origin = _rigidbody.position;
            Vector2 rawDirection = worldTarget - origin;

            if (rawDirection.sqrMagnitude <= 0.0001f)
                return Vector2.up;

            Vector2 direction = rawDirection.normalized;

            if (direction.y < 0.25f)
            {
                direction.y = 0.25f;
                direction.Normalize();
            }

            return direction;
        }

        private void OnLanded()
        {
            Vector2 velocity = _rigidbody.linearVelocity;

            if (velocity.y < 0f)
            {
                velocity.y = 0f;
            }

            velocity.x *= _landingHorizontalDamping;

            _rigidbody.linearVelocity = velocity;
        }
    }
}

