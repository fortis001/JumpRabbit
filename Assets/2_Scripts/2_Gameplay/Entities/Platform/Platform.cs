using System;
using UnityEngine;


namespace JumpRabbit.GamePlay.Entities
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private CollisionRelay2D _collisionRelay;
        [SerializeField] private BoxCollider2D _bodyCollider;

        public event Action<Platform, bool> OnPlayerLanded;

        private bool _isVisited;
        private int _index;

        public int Index => _index;
        public float HalfWidth => _bodyCollider.bounds.extents.x;

        public void Init()
        {
            if (_collisionRelay == null)
            {
                Debug.LogError("CollisionRelay2D가 할당되지 않았습니다.", this);
                return;
            }

            _collisionRelay.OnCollisionEntered -= HandleCollisionEntered;
            _collisionRelay.OnCollisionEntered += HandleCollisionEntered;

        }

        public void Activate(int index)
        {
            _isVisited = false;
            _index = index;
        }

        public void Deactivate()
        {
            _isVisited = false;
            _index = -1;
            OnPlayerLanded = null;
        }

        private void HandleCollisionEntered(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Player"))
                return;

            bool isFirstVisit = !_isVisited;

            if (isFirstVisit)
            {
                _isVisited = true;
            }

            OnPlayerLanded?.Invoke(this, isFirstVisit);
        }

        private void OnDestroy()
        {
            if (_collisionRelay != null)
            {
                _collisionRelay.OnCollisionEntered -= HandleCollisionEntered;
            }
        }
    }
}

