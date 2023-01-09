using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class TimeSystem : MonoBehaviour, IPersistant
    {
        public struct TimeSystemPersistantData
        {
            public float CurrentTime;
            public float TimeScale;
        }

        static TimeSystem _instance;
        public static TimeSystem Instance => _instance;

        [SerializeField] InputActionReference _startAction;
        [SerializeField] float _timeScale = 1f;
        float _currentTime;

        public int Day => (int)(_currentTime / (60f * 24f));
        public int Hour => (int)(_currentTime % (60f * 24f) / 60f);
        public int Minute => (int)(_currentTime % (60f));

        public void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                SetPause(false);
            }
        }

        void Update()
        {
            _currentTime += Time.deltaTime * _timeScale;
        }

        public void SetPause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
        }

        public object Save()
        {
            return new TimeSystemPersistantData
            {
                CurrentTime = _currentTime,
                TimeScale = 1f
            };
        }

        public void Load(object data, IGameDataHandler dataHandler)
        {
            var decoded = dataHandler.ToObject<TimeSystemPersistantData>(data);
            _currentTime = decoded.CurrentTime;
            Time.timeScale = decoded.TimeScale;
        }

        public Type GetPersistantDataType()
        {
            return typeof(TimeSystemPersistantData);
        }
    }
}
