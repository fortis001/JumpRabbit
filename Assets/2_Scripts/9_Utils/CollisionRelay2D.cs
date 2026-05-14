using System;
using UnityEngine;

namespace JumpRabbit.GamePlay
{
    public class CollisionRelay2D : MonoBehaviour
    {
        public event Action<Collision2D> OnCollisionEntered;
        public event Action<Collision2D> OnCollisionExited;
        public event Action<Collision2D> OnCollisionStayed;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnCollisionEntered?.Invoke(collision);
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            OnCollisionExited?.Invoke(collision);
        }

        private void OnCollisionStay2D(Collision2D collision)
        {
            OnCollisionStayed?.Invoke(collision);
        }
    }
}