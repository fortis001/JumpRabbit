using JumpRabbit.Core;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JumpRabbit.Actors.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Jump Charge")]
        [SerializeField] private float _minJumpPower = 5f;
        [SerializeField] private float _maxJumpPower = 12f;
        [SerializeField] private float _chargeDuration = 1f;


        private InputManager _inputManager;
        private PlayerMovement _movement;
        private PlayerSound _sound;

        private InputAction _jumpAction;

        private bool _isChargingJump;
        private float _chargeStartTime;
        private float _currentJumpPower;

        public void Init(InputManager inputManager, PlayerMovement movement, PlayerSound sound)
        {
            _inputManager = inputManager;
            _movement = movement;
            _sound = sound;

            _jumpAction = inputManager.GetAction("Jump");


            _jumpAction.started += OnJumpStarted;
            _jumpAction.canceled += OnJumpCanceled;
        }

        private void Update()
        {
            if (!_isChargingJump)
                return;

            float elapsed = TimeManager.Instance.GameTime.Time - _chargeStartTime;
            float t = Mathf.Clamp01(elapsed / _chargeDuration);

            _currentJumpPower = Mathf.Lerp(_minJumpPower, _maxJumpPower, t);
        }

        private void OnJumpStarted(InputAction.CallbackContext context)
        {
            if (!_movement.IsGrounded)
                return;

            _isChargingJump = true;
            _chargeStartTime = TimeManager.Instance.GameTime.Time;
            _currentJumpPower = _minJumpPower;
        }

        private void OnJumpCanceled(InputAction.CallbackContext context)
        {
            if (!_isChargingJump)
                return;

            _isChargingJump = false;

            Vector2 mouseWorldPosition = Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue());

            if (_movement.TryJumpTo(mouseWorldPosition, _currentJumpPower))
            {
                _sound.PlayJump();
            }

            _currentJumpPower = 0f;
        }

        private void OnDestroy()
        {
            if (_jumpAction == null)
                return;

            _jumpAction.started -= OnJumpStarted;
            _jumpAction.canceled -= OnJumpCanceled;
        }

    }
}

