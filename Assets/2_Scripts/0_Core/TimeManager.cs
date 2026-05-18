using System;
using LSH.Core;
using UnityEngine;

namespace JumpRabbit.Core
{
    public class TimeManager : Singleton<TimeManager>
    {

        public TimeChannel GameTime { get; } = new TimeChannel();
        public TimeChannel UITime { get; } = new TimeChannel();

        public bool IsGamePaused => GameTime.IsPaused;
        public bool IsUIPaused => UITime.IsPaused;

        public event Action OnGamePaused;
        public event Action OnGameResumed;

        private void Update()
        {
            float dt = Time.unscaledDeltaTime;

            GameTime.Tick(dt);
            UITime.Tick(dt);
        }

        public void PauseGame()
        {
            GameTime.Pause();
            OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            GameTime.Resume();
            OnGameResumed?.Invoke();
        }
    }
}