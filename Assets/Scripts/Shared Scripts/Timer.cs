using System;

namespace Game
{
    public class Timer
    {
		float _timeInSeconds = 0f;
		float _timeLeftInSeconds = 0f;

		public event Action OnElapsed;

		public Timer(float seconds)
		{
			_timeInSeconds = seconds;
		}

		public void Restart()
        {
			_timeLeftInSeconds = _timeInSeconds;
        }

		public void Update(float deltaTime)
        {
			_timeLeftInSeconds -= deltaTime;
			if (_timeLeftInSeconds <= 0f) OnElapsed?.Invoke();
		}
    }
}
