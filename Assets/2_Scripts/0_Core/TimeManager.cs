using LSH.Core;
using UnityEngine;

namespace JumpRabbit.Core
{
    public class TimeManager : Singleton<TimeManager>
    {

        public TimeChannel GameTime { get; } = new TimeChannel();
        public TimeChannel UITime { get; } = new TimeChannel();


        private void Update()
        {
            float dt = Time.unscaledDeltaTime;

            GameTime.Tick(dt);
            UITime.Tick(dt);
        }

        public void PauseGame()
        {
            GameTime.Pause();
        }

        public void ResumeGame()
        {
            GameTime.Resume();
        }
    }
}