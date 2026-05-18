using System;
using JumpRabbit.Actors.Player;
using LSH.Core;
using UnityEngine;
using UnityEngine.InputSystem;

namespace JumpRabbit.Actors
{
    public class PlayerActor : MonoBehaviour
    {
        [Header("Core Components")]
        [SerializeField] private PlayerController _controller; 
        [SerializeField] private PlayerMovement _movement;
        [SerializeField] private PlayerSound _sound;
        [SerializeField] private PlayerAnimation _animation;

        [Header("View")]
        [SerializeField] private Transform _body;
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;

        private float _gameOverY;

        #region Property
        public PlayerController Controller => _controller;
        public PlayerMovement Movement => _movement;
        public PlayerSound Sound => _sound;
        public PlayerAnimation Animation => _animation;
        public Transform Body => _body;
        public Animator Animator => _animator;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        #endregion

        public event Action OnPlayerFalled;

        public void Init(InputManager inputManager, float gameOverY)
        {
            _gameOverY = gameOverY;

            _controller.Init(inputManager, _movement, _sound, _animation);
            _animation.Init(_spriteRenderer);
            _movement.Init();
        }

        private void Update()
        {
            if(transform.position.y < _gameOverY)
            {
                OnPlayerFalled?.Invoke();
            }
        }
    }
}

