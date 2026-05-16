using System;
using System.Collections;
using System.Drawing;
using UnityEngine;


namespace JumpRabbit.GamePlay.Entities
{
    public class Platform : MonoBehaviour
    {
        [SerializeField] private CollisionRelay2D _collisionRelay;
        [SerializeField] private BoxCollider2D _bodyCollider;
        [SerializeField] private Animation _animation;

        public event Action<Platform, bool> OnPlayerLanded;

        private bool _isLandingEffectPlaying;

        private bool _isVisited;
        private int _index;
        private PlatformSize _size;
        private BonusCarrot _carrot;

        public int Index => _index;
        public PlatformSize Size => _size;
        public float HalfWidth => _bodyCollider.bounds.extents.x;
        public BonusCarrot Carrot => _carrot;

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

        public void Activate(int index, PlatformSize size)
        {
            _index = index;
            _size = size;
            _isVisited = false;

            gameObject.SetActive(true);
        }

        public void SetCarrot(BonusCarrot carrot)
        {
            _carrot = carrot;
            _carrot.OnCollected += HandleCarrotCollected;
        }

        private void HandleCarrotCollected(BonusCarrot carrot, float bonusRate)
        {
            _carrot.OnCollected -= HandleCarrotCollected;
            _carrot = null;
        }

        public void Deactivate()
        {
            _isVisited = false;
            _index = -1;
            OnPlayerLanded = null;

            if(_carrot != null)
            {
                Destroy(_carrot.gameObject);
            }

            gameObject.SetActive(false);
        }

        private void HandleCollisionEntered(Collision2D collision)
        {
            if (!collision.collider.CompareTag("Player"))
                return;

            if (_isLandingEffectPlaying)
                return;

            StartCoroutine(PlayLandingEffect());

            bool isFirstVisit = !_isVisited;

            if (isFirstVisit)
            {
                _isVisited = true;
            }

            OnPlayerLanded?.Invoke(this, isFirstVisit);
        }

        private IEnumerator PlayLandingEffect()
        {
            _isLandingEffectPlaying = true;

            _animation.Play();

            yield return new WaitForSeconds(_animation.clip.length);

            _isLandingEffectPlaying = false;
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

