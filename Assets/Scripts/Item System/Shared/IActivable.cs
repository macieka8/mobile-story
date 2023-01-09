using UnityEngine;
using System;

namespace Game
{
    public interface IActivable
    {
        public string name { get; }
        public Sprite Icon { get; }
        public float CooldownInSeconds { get; }
        public void Activate(GameObject target);

        public event Action<IActivable> OnActivableRemove;
        public event Action OnActivate;
    }
}
