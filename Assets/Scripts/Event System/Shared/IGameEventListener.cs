﻿namespace Game
{
    public interface IGameEventListener<T>
    {
        public void OnEventRaised(T value);
    }
}
