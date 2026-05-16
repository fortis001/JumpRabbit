using System;
using UnityEngine;

namespace JumpRabbit.Utils
{
    public class TriggerRelay2D : MonoBehaviour
    {
        public event Action<Collider2D> OnTriggerEntered;
        public event Action<Collider2D> OnTriggerExited;
        public event Action<Collider2D> OnTriggerStayed;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnTriggerEntered?.Invoke(other);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            OnTriggerExited?.Invoke(other);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            OnTriggerStayed?.Invoke(other);
        }
    }
}