using System;

namespace Game
{
    [Serializable]
    public abstract class ActivableReference
    {
        public abstract IActivable GetActivable();
    }
}
