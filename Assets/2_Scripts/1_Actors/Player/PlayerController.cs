using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JumpRabbit.Actors.Player
{
    public class PlayerController : MonoBehaviour
    {     
        [Header("Jump Charge")]
        [SerializeField] private float _minJumpPower = 6.5f;
        [SerializeField] private float _maxJumpPower = 20f;
        [SerializeField] private float _chargeDuration = 1f;


        private InputManager _inputManager;
        private PlayerMovement _movement;
        private PlayerSound _sound;
        private PlayerAnimation _animation;
        private Camera _mainCamera;

        private InputAction _jumpAction;

        private bool _isChargingJump;
        private float _chargeStartTime;
        private float _currentJumpPower;

        public void Init(InputManager inputManager, PlayerMovement movement, PlayerSound sound, PlayerAnimation animation)
        {
            _inputManager = inputManager;
            _movement = movement;
            _sound = sound;
            _animation = animation;
            _mainCamera = Camera.main;

            _jumpAction = inputManager.GetAction(InputActionName.Jump);

            _jumpAction.started += HandleJumpStarted;
            _jumpAction.canceled += HandleJumpCanceled;

            _movement.OnLanded += HandleLanded;
        }

        private void Update()
        {
            if (TimeManager.Instance.IsGamePaused) return;

            UpdateLookDirection();

            if (_isChargingJump)
            {
                UpdateJumpCharge();
            }
        }

        private void UpdateLookDirection()
        {
            if (!_movement.IsGrounded)
                return;

            Vector2 mouseWorldPosition = GetMouseWorldPosition();
            _animation.LookAt(mouseWorldPosition);
        }

        private void UpdateJumpCharge()
        {
            float elapsed = TimeManager.Instance.GameTime.Time - _chargeStartTime;
            float t = Mathf.Clamp01(elapsed / _chargeDuration);

            _currentJumpPower = Mathf.Lerp(_minJumpPower, _maxJumpPower, t);
        }

        private void HandleJumpStarted(InputAction.CallbackContext context)
        {
            if (!_movement.IsGrounded)
                return;

            _isChargingJump = true;
            _chargeStartTime = TimeManager.Instance.GameTime.Time;
            _currentJumpPower = _minJumpPower;

            _animation.PlayReadyJump();
        }

        private void HandleJumpCanceled(InputAction.CallbackContext context)
        {
            if (!_isChargingJump)
                return;

            _isChargingJump = false;

            Vector2 mouseWorldPosition = GetMouseWorldPosition();

            if (_movement.TryJumpTo(mouseWorldPosition, _currentJumpPower))
            {
                _animation.PlayJump();
                _sound.PlayJump();
            }

            _currentJumpPower = 0f;
        }

        private void HandleLanded()
        {
            _animation.PlayIdle();
        }

        private Vector2 GetMouseWorldPosition()
        {
            return _mainCamera.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());
        }

        private void OnDestroy()
        {
            if (_jumpAction == null)
                return;

            _jumpAction.started -= HandleJumpStarted;
            _jumpAction.canceled -= HandleJumpCanceled;
        }

    }
}

