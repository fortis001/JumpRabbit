using System;
using JumpRabbit.Utils;
using UnityEngine;

namespace JumpRabbit.GamePlay.Entities
{
    public class BonusCarrot : MonoBehaviour
    {
        [SerializeField] private TriggerRelay2D _triggerRelay;
        [SerializeField] private CircleCollider2D _bodyCollider;

        [SerializeField] private float _additionalBonusRate = 0.3f;

        public event Action<BonusCarrot, float> OnCollected;

        public void Init()
        {
            if (_triggerRelay == null)
            {
                Debug.LogError("TriggerRelay2D가 할당되지 않았습니다.", this);
                return;
            }

            _triggerRelay.OnTriggerEntered -= HandleTriggerEntered;
            _triggerRelay.OnTriggerEntered += HandleTriggerEntered;
        }

        private void HandleTriggerEntered(Collider2D other)
        {
            if (!other.CompareTag("Player"))
                return;

            OnCollected?.Invoke(this, _additionalBonusRate);
        }

        private void OnDestroy()
        {
            if (_triggerRelay != null)
            {
                _triggerRelay.OnTriggerEntered -= HandleTriggerEntered;
            }
        }
    }

}
