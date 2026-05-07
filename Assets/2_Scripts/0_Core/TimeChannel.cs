using UnityEngine;

namespace JumpRabbit.Core
{
    public sealed class TimeChannel
    {
        public float Time { get; private set; }
        public float DeltaTime { get; private set; }
        public float Scale { get; private set; } = 1f;
        public bool IsPaused { get; private set; }

        public void Tick(float unscaledDeltaTime)
        {
            if (IsPaused)
            {
                DeltaTime = 0f;
                return;
            }

            DeltaTime = unscaledDeltaTime * Scale;
            Time += DeltaTime;
        }

        public void Pause()
        {
            IsPaused = true;
            DeltaTime = 0f;
        }

        public void Resume()
        {
            IsPaused = false;
        }

        public void SetScale(float scale)
        {
            Scale = Mathf.Max(0f, scale);
        }

        public void Reset()
        {
            Time = 0f;
            DeltaTime = 0f;
            Scale = 1f;
            IsPaused = false;
        }
    }
}